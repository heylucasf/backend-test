using MediatR;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;
using Serilog;

using FluentValidation;
using ProjInv.Application.DTOs.Responses.Investimento;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.Application.UseCases.Investimento.Handles
{
    public class RetirarInvestimentoHandler : IRequestHandler<RetirarInvestimentoCommand, RetiradaDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<RetirarInvestimentoCommand> _validator;
        private readonly ILogger _logger;

        public RetirarInvestimentoHandler(
            IUnitOfWork uow,
            IValidator<RetirarInvestimentoCommand> validator,
            ILogger logger)
        {
            _uow = uow;
            _validator = validator;
            _logger = logger;
        }

        public async Task<RetiradaDto> Handle(RetirarInvestimentoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }

            _logger.Information("Processando retirada do investimento {InvestimentoId}", request.InvestimentoId);

            await _uow.BeginTransactionAsync();

            try
            {
                var investimento = await _uow.Investimentos.GetByIdAsync(request.InvestimentoId, cancellationToken);
                if (investimento == null)
                {
                    _logger.Warning("Investimento {InvestimentoId} nao encontrado", request.InvestimentoId);
                    throw new KeyNotFoundException($"Investimento com ID {request.InvestimentoId} nao encontrado.");
                }

                if (investimento.FoiRetirado)
                {
                    _logger.Warning("Investimento {InvestimentoId} ja foi retirado", request.InvestimentoId);
                    throw new InvalidOperationException("Este investimento ja foi retirado.");
                }

                if (request.DataRetirada < investimento.DataCriacao)
                {
                    throw new ArgumentException("A data de retirada nao pode ser anterior a data de criacao do investimento.");
                }

                if (request.DataRetirada > DateTime.UtcNow.Date)
                {
                    throw new ArgumentException("A data de retirada nao pode ser no futuro.");
                }

                var valorBruto = investimento.CalcularSaldoEsperado(request.DataRetirada);
                var impostos = investimento.CalcularImpostos(request.DataRetirada);
                var valorLiquido = valorBruto - impostos;

                var retirada = new Retirada(request.InvestimentoId, request.DataRetirada, valorBruto, impostos, valorLiquido);
                await _uow.Retiradas.AddAsync(retirada, cancellationToken);

                await _uow.Investimentos.UpdateAsync(investimento, cancellationToken);

                await _uow.CommitAsync();

                _logger.Information("Retirada {RetiradaId} criada com sucesso para investimento {InvestimentoId}", retirada.Id, request.InvestimentoId);

                return new RetiradaDto
                {
                    Id = retirada.Id,
                    DataRetirada = retirada.DataRetirada,
                    ValorBruto = retirada.ValorBruto,
                    Impostos = retirada.Impostos,
                    ValorLiquido = retirada.ValorLiquido
                };
            }
            catch
            {
                await _uow.RollbackAsync();
                _logger.Error("Erro ao processar retirada do investimento {InvestimentoId}", request.InvestimentoId);
                throw;
            }
        }
    }
}

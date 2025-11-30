using MediatR;
using ProjInv.Domain.Interfaces;
using Serilog;
using FluentValidation;
using ProjInv.Application.DTOs.Responses.Investimento;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.Application.UseCases.Investimento.Handles
{
    public class CriarInvestimentoHandler : IRequestHandler<CriarInvestimentoCommand, InvestimentoResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CriarInvestimentoCommand> _validator;
        private readonly ILogger _logger;

        public CriarInvestimentoHandler(
            IUnitOfWork unitOfWork,
            IValidator<CriarInvestimentoCommand> validator,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _logger = logger;
        }

        public async Task<InvestimentoResponseDto> Handle(CriarInvestimentoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }

            _logger.Information("Criando investimento para investidor {InvestidorId} com valor {Valor}", request.InvestidorId, request.ValorInicial);

            var investidor = await _unitOfWork.Investidores.GetByIdAsync(request.InvestidorId, cancellationToken);
            if (investidor == null)
            {
                _logger.Warning("Investidor {InvestidorId} nao encontrado", request.InvestidorId);
                throw new KeyNotFoundException($"Investidor com ID {request.InvestidorId} nao encontrado.");
            }

            var investimento = new Domain.Entities.Investimento(request.InvestidorId, request.ValorInicial, request.DataCriacao);
            await _unitOfWork.Investimentos.AddAsync(investimento, cancellationToken);

            // Commit através do UnitOfWork para garantir persistência transacional
            await _unitOfWork.CommitAsync();

            return new InvestimentoResponseDto
            {
                Id = investimento.Id,
                InvestidorId = investimento.InvestidorId,
                ValorInicial = investimento.ValorInicial,
                DataCriacao = investimento.DataCriacao,
                SaldoEsperado = investimento.CalcularSaldoEsperado(),
                FoiRetirado = false
            };
        }
    }
}

using MediatR;
using ProjInv.Domain.Interfaces;

using FluentValidation;
using ProjInv.Application.DTOs.Responses.Investimento;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.Application.UseCases.Investimento.Handles
{
    public class VisualizarInvestimentoHandler : IRequestHandler<VisualizarInvestimentoCommand, InvestimentoResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<VisualizarInvestimentoCommand> _validator;

        public VisualizarInvestimentoHandler(IUnitOfWork uow, IValidator<VisualizarInvestimentoCommand> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<InvestimentoResponseDto> Handle(VisualizarInvestimentoCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }

            var investimento = await _uow.Investimentos.GetByIdAsync(request.Id, cancellationToken);
            if (investimento == null)
            {
                throw new KeyNotFoundException($"Investimento com ID {request.Id} nao encontrado.");
            }

            RetiradaDto? retiradaDto = null;
            if (investimento.FoiRetirado && investimento.Retirada != null)
            {
                retiradaDto = new RetiradaDto
                {
                    Id = investimento.Retirada.Id,
                    DataRetirada = investimento.Retirada.DataRetirada,
                    ValorBruto = investimento.Retirada.ValorBruto,
                    Impostos = investimento.Retirada.Impostos,
                    ValorLiquido = investimento.Retirada.ValorLiquido
                };
            }

            return new InvestimentoResponseDto
            {
                Id = investimento.Id,
                InvestidorId = investimento.InvestidorId,
                ValorInicial = investimento.ValorInicial,
                DataCriacao = investimento.DataCriacao,
                SaldoEsperado = investimento.CalcularSaldoEsperado(),
                FoiRetirado = investimento.FoiRetirado,
                Retirada = retiradaDto
            };
        }
    }
}

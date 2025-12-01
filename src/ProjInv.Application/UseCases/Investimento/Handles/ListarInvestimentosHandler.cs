using MediatR;
using ProjInv.Domain.Interfaces;

using FluentValidation;
using ProjInv.Application.DTOs.Responses.Investimento;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.Application.UseCases.Investimento.Handles
{
    public class ListarInvestimentosHandler : IRequestHandler<ListarInvestimentosCommand, ListarInvestimentosResponseDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<ListarInvestimentosCommand> _validator;

        public ListarInvestimentosHandler(IUnitOfWork uow, IValidator<ListarInvestimentosCommand> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<ListarInvestimentosResponseDto> Handle(ListarInvestimentosCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }

            var investimentos = await _uow.Investimentos.GetByInvestidorIdAsync(request.InvestidorId, request.Page, request.PageSize, cancellationToken);
            var totalCount = await _uow.Investimentos.CountByInvestidorIdAsync(request.InvestidorId, cancellationToken);

            var investimentosDto = investimentos.Select(i =>
            {
                RetiradaDto? retiradaDto = null;
                if (i.FoiRetirado)
                {
                    retiradaDto = new RetiradaDto
                    {
                        Id = i.Retirada!.Id,
                        DataRetirada = i.Retirada.DataRetirada,
                        ValorBruto = i.Retirada.ValorBruto,
                        Impostos = i.Retirada.Impostos,
                        ValorLiquido = i.Retirada.ValorLiquido
                    };
                }

                return new InvestimentoResponseDto
                {
                    Id = i.Id,
                    InvestidorId = i.InvestidorId,
                    ValorInicial = i.ValorInicial,
                    DataCriacao = i.DataCriacao,
                    SaldoEsperado = i.CalcularSaldoEsperado(),
                    FoiRetirado = i.FoiRetirado,
                    Retirada = retiradaDto
                };
            }).ToList();

            return new ListarInvestimentosResponseDto
            {
                Investimentos = investimentosDto,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}

using FluentValidation;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.Application.UseCases.Investimento.Validators
{
    public class CriarInvestimentoCommandValidator : AbstractValidator<CriarInvestimentoCommand>
    {
        public CriarInvestimentoCommandValidator()
        {
            RuleFor(x => x.InvestidorId)
                .NotEmpty().WithMessage("O ID do investidor e obrigatorio.");

            RuleFor(x => x.ValorInicial)
                .GreaterThan(0).WithMessage("O valor inicial deve ser maior que zero.");

            RuleFor(x => x.DataCriacao)
                .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("A data de criacao nao pode ser no futuro.");
        }
    }
}

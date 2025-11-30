using FluentValidation;
using ProjInv.Application.UseCases.Investidor.Commands;

namespace ProjInv.Application.UseCases.Investidor.Validators
{
    public class CriarInvestidorCommandValidator : AbstractValidator<CriarInvestidorCommand>
    {
        public CriarInvestidorCommandValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do investidor e obrigatorio.")
                .Length(3, 200).WithMessage("O nome deve ter entre 3 e 200 caracteres.");
        }
    }
}

using FluentValidation;
using ProjInv.Application.UseCases.Investidor.Commands;

namespace ProjInv.Application.UseCases.Investidor.Validators
{
    public class UpdateInvestidorCommandValidator : AbstractValidator<UpdateInvestidorCommand>
    {
        public UpdateInvestidorCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do investidor e obrigatorio.");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do investidor e obrigatorio.")
                .Length(3, 200).WithMessage("O nome deve ter entre 3 e 200 caracteres.");
        }
    }
}

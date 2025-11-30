using FluentValidation;
using ProjInv.Application.UseCases.Investidor.Commands;

namespace ProjInv.Application.UseCases.Investidor.Validators
{
    public class DeleteInvestidorCommandValidator : AbstractValidator<DeleteInvestidorCommand>
    {
        public DeleteInvestidorCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do investidor e obrigatorio.");
        }
    }
}

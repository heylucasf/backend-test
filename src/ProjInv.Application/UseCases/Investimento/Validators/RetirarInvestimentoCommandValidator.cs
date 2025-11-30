using FluentValidation;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.Application.UseCases.Investimento.Validators
{
    public class RetirarInvestimentoCommandValidator : AbstractValidator<RetirarInvestimentoCommand>
    {
        public RetirarInvestimentoCommandValidator()
        {
            RuleFor(x => x.InvestimentoId)
                .NotEmpty().WithMessage("O ID do investimento e obrigatorio.");

            RuleFor(x => x.DataRetirada)
                .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("A data de retirada nao pode ser no futuro.");
        }
    }
}

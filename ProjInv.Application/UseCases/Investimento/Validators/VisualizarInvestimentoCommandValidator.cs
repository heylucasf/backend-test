using FluentValidation;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.Application.UseCases.Investimento.Validators
{
    public class VisualizarInvestimentoCommandValidator : AbstractValidator<VisualizarInvestimentoCommand>
    {
        public VisualizarInvestimentoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do investimento e obrigatorio.");
        }
    }
}

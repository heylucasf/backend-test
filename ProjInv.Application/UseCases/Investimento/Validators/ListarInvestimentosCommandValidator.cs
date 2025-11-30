using FluentValidation;
using ProjInv.Application.UseCases.Investimento.Commands;

namespace ProjInv.Application.UseCases.Investimento.Validators
{
    public class ListarInvestimentosCommandValidator : AbstractValidator<ListarInvestimentosCommand>
    {
        public ListarInvestimentosCommandValidator()
        {
            RuleFor(x => x.InvestidorId)
                .NotEmpty().WithMessage("O ID do investidor e obrigatorio.");

            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("A pagina deve ser maior que zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("O tamanho da pagina deve ser maior que zero.");
        }
    }
}

using FluentValidation;
using ProjInv.Application.UseCases.Investidor.Commands;

namespace ProjInv.Application.UseCases.Investidor.Validators
{
    public class GetAllInvestidorCommandValidator : AbstractValidator<GetAllInvestidorCommand>
    {
        public GetAllInvestidorCommandValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("A pagina deve ser maior que zero.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("O tamanho da pagina deve ser maior que zero.");
        }
    }
}

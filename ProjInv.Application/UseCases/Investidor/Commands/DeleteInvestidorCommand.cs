using MediatR;

namespace ProjInv.Application.UseCases.Investidor.Commands
{
    public class DeleteInvestidorCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
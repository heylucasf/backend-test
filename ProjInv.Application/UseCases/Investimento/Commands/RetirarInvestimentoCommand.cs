using MediatR;
using ProjInv.Application.DTOs.Responses.Investimento;

namespace ProjInv.Application.UseCases.Investimento.Commands
{
    public class RetirarInvestimentoCommand : IRequest<RetiradaDto>
    {
        public Guid InvestimentoId { get; set; }
        public DateTime DataRetirada { get; set; }
    }
}

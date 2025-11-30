using MediatR;
using ProjInv.Application.DTOs.Responses.Investimento;

namespace ProjInv.Application.UseCases.Investimento.Commands
{
    public class VisualizarInvestimentoCommand : IRequest<InvestimentoResponseDto>
    {
        public Guid Id { get; set; }
    }
}

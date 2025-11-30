using MediatR;
using ProjInv.Application.DTOs.Responses.Investimento;

namespace ProjInv.Application.UseCases.Investimento.Commands
{
    public class ListarInvestimentosCommand : IRequest<ListarInvestimentosResponseDto>
    {
        public Guid InvestidorId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

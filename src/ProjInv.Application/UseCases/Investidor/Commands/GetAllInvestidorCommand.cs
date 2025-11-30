using MediatR;
using ProjInv.Application.DTOs.Responses.Investidor;

namespace ProjInv.Application.UseCases.Investidor.Commands
{
    public class GetAllInvestidorCommand : IRequest<GetAllInvestidorResponseDto>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

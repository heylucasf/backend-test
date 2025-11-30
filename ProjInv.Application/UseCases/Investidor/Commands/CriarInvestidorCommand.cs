using MediatR;
using ProjInv.Application.DTOs.Responses.Investidor;

namespace ProjInv.Application.UseCases.Investidor.Commands
{
    public class CriarInvestidorCommand : IRequest<InvestidorResponseDto>
    {
        public string Nome { get; set; } = string.Empty;
    }
}

using MediatR;
using ProjInv.Application.DTOs.Responses.Investimento;

namespace ProjInv.Application.UseCases.Investimento.Commands
{
    public class CriarInvestimentoCommand : IRequest<InvestimentoResponseDto>
    {
        public Guid InvestidorId { get; set; }
        public decimal ValorInicial { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}

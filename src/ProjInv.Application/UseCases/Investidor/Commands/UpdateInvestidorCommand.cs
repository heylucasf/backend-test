using MediatR;
using ProjInv.Application.DTOs.Responses.Investidor;

namespace ProjInv.Application.UseCases.Investidor.Commands
{
    public class UpdateInvestidorCommand : IRequest<InvestidorResponseDto>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public UpdateInvestidorCommand(Guid id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}
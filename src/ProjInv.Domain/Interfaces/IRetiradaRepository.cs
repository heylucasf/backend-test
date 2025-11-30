using ProjInv.Domain.Entities;

namespace ProjInv.Domain.Interfaces
{
    public interface IRetiradaRepository
    {
        Task<Retirada?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Retirada?> GetByInvestimentoIdAsync(Guid investimentoId, CancellationToken cancellationToken = default);
        Task AddAsync(Retirada retirada, CancellationToken cancellationToken = default);
    }
}

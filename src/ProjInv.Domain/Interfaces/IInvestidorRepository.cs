using ProjInv.Domain.Entities;

namespace ProjInv.Domain.Interfaces
{
    public interface IInvestidorRepository
    {
        Task<Investidor?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<Investidor>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task AddAsync(Investidor investidor, CancellationToken cancellationToken);
        Task UpdateAsync(Investidor investidor, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}

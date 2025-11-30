using ProjInv.Domain.Entities;

namespace ProjInv.Domain.Interfaces
{
    public interface IInvestimentoRepository
    {
        Task<Investimento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Investimento>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Investimento>> GetByInvestidorIdAsync(Guid investidorId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<int> CountByInvestidorIdAsync(Guid investidorId, CancellationToken cancellationToken = default);
        Task AddAsync(Investimento investimento, CancellationToken cancellationToken = default);
        Task UpdateAsync(Investimento investimento, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

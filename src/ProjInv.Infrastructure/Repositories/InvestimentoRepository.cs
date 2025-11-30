using Microsoft.EntityFrameworkCore;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;
using ProjInv.Infrastructure.Data;

namespace ProjInv.Infrastructure.Repositories
{
    public class InvestimentoRepository : IInvestimentoRepository
    {
        private readonly AppDbContext _context;

        public InvestimentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Investimento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Investimentos
                .Include(i => i.Investidor)
                .Include(i => i.Retirada)
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Investimento>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Investimentos
                .Include(i => i.Investidor)
                .Include(i => i.Retirada)
                .OrderByDescending(i => i.DataCriacao)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Investimento>> GetByInvestidorIdAsync(Guid investidorId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Investimentos
                .Where(i => i.InvestidorId == investidorId)
                .Include(i => i.Retirada)
                .OrderByDescending(i => i.DataCriacao)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountByInvestidorIdAsync(Guid investidorId, CancellationToken cancellationToken = default)
        {
            return await _context.Investimentos
                .CountAsync(i => i.InvestidorId == investidorId, cancellationToken);
        }

        public async Task AddAsync(Investimento investimento, CancellationToken cancellationToken = default)
        {
            await _context.Investimentos.AddAsync(investimento, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Investimento investimento, CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var investimento = await GetByIdAsync(id, cancellationToken);
            if (investimento != null)
            {
                _context.Investimentos.Remove(investimento);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

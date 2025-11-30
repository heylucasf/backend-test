using Microsoft.EntityFrameworkCore;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;
using ProjInv.Infrastructure.Data;

namespace ProjInv.Infrastructure.Repositories
{
    public class InvestidorRepository : IInvestidorRepository
    {
        private readonly AppDbContext _context;

        public InvestidorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Investidor?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Investidores
                .Include(i => i.Investimentos)
                    .ThenInclude(inv => inv.Retirada)
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }
        public async Task<IEnumerable<Investidor>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Investidores
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddAsync(Investidor investidor, CancellationToken cancellationToken)
        {
            await _context.Investidores.AddAsync(investidor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Investidor investidor, CancellationToken cancellationToken)
        {
            _context.Investidores.Update(investidor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken) {
            var investidor = await _context.Investidores.FindAsync(id);
            if (investidor != null)
            {
                _context.Investidores.Remove(investidor);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

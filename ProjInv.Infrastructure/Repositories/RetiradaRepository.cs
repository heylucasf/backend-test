using Microsoft.EntityFrameworkCore;
using ProjInv.Domain.Entities;
using ProjInv.Domain.Interfaces;
using ProjInv.Infrastructure.Data;

namespace ProjInv.Infrastructure.Repositories
{
    public class RetiradaRepository : IRetiradaRepository
    {
        private readonly AppDbContext _context;

        public RetiradaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Retirada?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Retiradas
                .Include(r => r.Investimento)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<Retirada?> GetByInvestimentoIdAsync(Guid investimentoId, CancellationToken cancellationToken = default)
        {
            return await _context.Retiradas
                .FirstOrDefaultAsync(r => r.InvestimentoId == investimentoId, cancellationToken);
        }

        public async Task AddAsync(Retirada retirada, CancellationToken cancellationToken = default)
        {
            await _context.Retiradas.AddAsync(retirada, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

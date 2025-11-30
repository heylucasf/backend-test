using Microsoft.EntityFrameworkCore.Storage;
using ProjInv.Domain.Interfaces;
using ProjInv.Infrastructure.Data;

namespace ProjInv.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IInvestidorRepository Investidores { get; }
        public IInvestimentoRepository Investimentos { get; }
        public IRetiradaRepository Retiradas { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Investidores = new InvestidorRepository(context);
            Investimentos = new InvestimentoRepository(context);
            Retiradas = new RetiradaRepository(context);
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

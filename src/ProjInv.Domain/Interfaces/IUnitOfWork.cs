namespace ProjInv.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IInvestidorRepository Investidores { get; }
        IInvestimentoRepository Investimentos { get; }
        IRetiradaRepository Retiradas { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}

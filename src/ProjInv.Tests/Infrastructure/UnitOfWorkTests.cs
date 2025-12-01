using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProjInv.Domain.Entities;
using ProjInv.Infrastructure.Data;
using ProjInv.Infrastructure.Repositories;

namespace ProjInv.Tests.Infrastructure
{
    public class UnitOfWorkTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public UnitOfWorkTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }

        [Fact]
        public void Deve_Expor_Repositorios_Corretamente()
        {
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);

            uow.Investidores.Should().NotBeNull();
            uow.Investimentos.Should().NotBeNull();
            uow.Retiradas.Should().NotBeNull();
        }

        [Fact]
        public async Task Deve_Salvar_Mudancas_Com_CommitAsync()
        {
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);

            var investidor = new Investidor("Lucas");
            await uow.Investidores.AddAsync(investidor, CancellationToken.None);
            await context.SaveChangesAsync();

            var saved = await context.Investidores.FirstOrDefaultAsync();
            saved.Should().NotBeNull();
            saved!.Nome.Should().Be("Lucas");
        }

        [Fact]
        public async Task Deve_Iniciar_E_Fazer_Commit_De_Transacao()
        {
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);

            await uow.BeginTransactionAsync();
            await uow.CommitAsync();

            context.ChangeTracker.HasChanges().Should().BeFalse();
        }

        [Fact]
        public async Task Deve_Fazer_Rollback_De_Transacao()
        {
            using var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);

            await uow.BeginTransactionAsync();
            await uow.RollbackAsync();

            context.ChangeTracker.HasChanges().Should().BeFalse();
        }

        [Fact]
        public void Deve_Fazer_Dispose_Corretamente()
        {
            var context = new AppDbContext(_options);
            var uow = new UnitOfWork(context);

            uow.Dispose();

            var action = () => context.Investidores.ToList();
            action.Should().Throw<ObjectDisposedException>();
        }
    }
}

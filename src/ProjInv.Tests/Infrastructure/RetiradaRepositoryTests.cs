using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProjInv.Domain.Entities;
using ProjInv.Infrastructure.Data;
using ProjInv.Infrastructure.Repositories;

namespace ProjInv.Tests.Infrastructure
{
    public class RetiradaRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public RetiradaRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Deve_Adicionar_Retirada()
        {
            using var context = new AppDbContext(_options);
            var repository = new RetiradaRepository(context);
            var retirada = new Retirada(Guid.NewGuid(), DateTime.UtcNow, 1000, 100, 900);

            await repository.AddAsync(retirada);
            await context.SaveChangesAsync();

            var saved = await context.Retiradas.FirstOrDefaultAsync();
            saved.Should().NotBeNull();
            saved!.ValorBruto.Should().Be(1000);
        }
    }
}

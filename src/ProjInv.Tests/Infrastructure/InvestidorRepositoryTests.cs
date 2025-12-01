using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProjInv.Domain.Entities;
using ProjInv.Infrastructure.Data;
using ProjInv.Infrastructure.Repositories;

namespace ProjInv.Tests.Infrastructure
{
    public class InvestidorRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public InvestidorRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Deve_Adicionar_Investidor()
        {
            using var context = new AppDbContext(_options);
            var repository = new InvestidorRepository(context);
            var investidor = new Investidor("Lucas");

            await repository.AddAsync(investidor, CancellationToken.None);
            await context.SaveChangesAsync();

            var saved = await context.Investidores.FirstOrDefaultAsync();
            saved.Should().NotBeNull();
            saved!.Nome.Should().Be("Lucas");
        }

        [Fact]
        public async Task Deve_Obter_Investidor_Por_Id()
        {
            using var context = new AppDbContext(_options);
            var repository = new InvestidorRepository(context);
            var investidor = new Investidor("Lucas");
            await context.Investidores.AddAsync(investidor);
            await context.SaveChangesAsync();

            var result = await repository.GetByIdAsync(investidor.Id, CancellationToken.None);

            result.Should().NotBeNull();
            result!.Id.Should().Be(investidor.Id);
        }
    }
}

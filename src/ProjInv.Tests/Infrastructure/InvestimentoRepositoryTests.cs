using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProjInv.Domain.Entities;
using ProjInv.Infrastructure.Data;
using ProjInv.Infrastructure.Repositories;

namespace ProjInv.Tests.Infrastructure
{
    public class InvestimentoRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public InvestimentoRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Deve_Adicionar_Investimento()
        {
            using var context = new AppDbContext(_options);
            var repository = new InvestimentoRepository(context);
            var investimento = new Investimento(Guid.NewGuid(), 1000m, DateTime.UtcNow);

            await repository.AddAsync(investimento);
            await context.SaveChangesAsync();

            var saved = await context.Investimentos.FirstOrDefaultAsync();
            saved.Should().NotBeNull();
            saved!.ValorInicial.Should().Be(1000m);
        }

        [Fact]
        public async Task Deve_Listar_Investimentos_Por_Investidor_Paginado()
        {
            using var context = new AppDbContext(_options);
            var repository = new InvestimentoRepository(context);
            var investidorId = Guid.NewGuid();
            
            for (int i = 0; i < 15; i++)
            {
                await context.Investimentos.AddAsync(new Investimento(investidorId, 1000 + i, DateTime.UtcNow));
            }
            await context.SaveChangesAsync();

            var result = await repository.GetByInvestidorIdAsync(investidorId, 1, 10);

            result.Should().HaveCount(10);
        }
    }
}

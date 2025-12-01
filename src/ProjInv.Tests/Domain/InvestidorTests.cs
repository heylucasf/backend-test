using FluentAssertions;
using ProjInv.Domain.Entities;

namespace ProjInv.Tests.Domain
{
    public class InvestidorTests
    {
        [Fact]
        public void Deve_Criar_Investidor_Com_Sucesso()
        {
            var nome = "Lucas";
            var investidor = new Investidor(nome);

            investidor.Should().NotBeNull();
            investidor.Id.Should().NotBeEmpty();
            investidor.Nome.Should().Be(nome);
            investidor.Investimentos.Should().BeEmpty();
        }

        [Fact]
        public void Deve_Atualizar_Nome_Investidor()
        {
            var investidor = new Investidor("Lucas");
            var novoNome = "Lucas Updated";

            investidor.UpdateNome(novoNome);

            investidor.Nome.Should().Be(novoNome);
        }
    }
}

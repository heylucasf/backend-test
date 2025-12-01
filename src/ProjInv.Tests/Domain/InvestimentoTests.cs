using FluentAssertions;
using ProjInv.Domain.Entities;

namespace ProjInv.Tests.Domain
{
    public class InvestimentoTests
    {
        [Fact]
        public void Deve_Criar_Investimento_Com_Sucesso()
        {
            var investidorId = Guid.NewGuid();
            var valorInicial = 1000m;
            var dataCriacao = DateTime.UtcNow.Date;

            var investimento = new Investimento(investidorId, valorInicial, dataCriacao);

            investimento.Should().NotBeNull();
            investimento.InvestidorId.Should().Be(investidorId);
            investimento.ValorInicial.Should().Be(valorInicial);
            investimento.DataCriacao.Should().Be(dataCriacao);
        }

        [Fact]
        public void Nao_Deve_Criar_Investimento_Com_Valor_Negativo_Ou_Zero()
        {
            var investidorId = Guid.NewGuid();
            var dataCriacao = DateTime.UtcNow.Date;

            Assert.Throws<ArgumentException>(() => new Investimento(investidorId, 0, dataCriacao));
            Assert.Throws<ArgumentException>(() => new Investimento(investidorId, -100, dataCriacao));
        }

        [Fact]
        public void Nao_Deve_Criar_Investimento_No_Futuro()
        {
            var investidorId = Guid.NewGuid();
            var valorInicial = 1000m;
            var dataFutura = DateTime.UtcNow.AddDays(1);

            Assert.Throws<ArgumentException>(() => new Investimento(investidorId, valorInicial, dataFutura));
        }

        [Fact]
        public void Deve_Calcular_Ganhos_Corretamente()
        {
            var investidorId = Guid.NewGuid();
            var valorInicial = 1000m;
            var dataCriacao = DateTime.UtcNow.AddMonths(-1);
            var investimento = new Investimento(investidorId, valorInicial, dataCriacao);

            var ganhos = investimento.CalcularGanhos(DateTime.UtcNow);

            ganhos.Should().Be(5.20m);
        }

        [Fact]
        public void Deve_Calcular_Saldo_Esperado_Corretamente()
        {
            var investidorId = Guid.NewGuid();
            var valorInicial = 1000m;
            var dataCriacao = DateTime.UtcNow.AddMonths(-1);
            var investimento = new Investimento(investidorId, valorInicial, dataCriacao);

            var saldo = investimento.CalcularSaldoEsperado(DateTime.UtcNow);

            saldo.Should().Be(1005.20m);
        }
    }
}

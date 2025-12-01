using FluentAssertions;
using ProjInv.Domain.Entities;

namespace ProjInv.Tests.Domain
{
    public class RetiradaTests
    {
        [Fact]
        public void Deve_Calcular_Imposto_22_5_Porcento_Para_Menos_De_Um_Ano()
        {
            var investidorId = Guid.NewGuid();
            var valorInicial = 1000m;
            var dataCriacao = DateTime.UtcNow.AddMonths(-6);
            var investimento = new Investimento(investidorId, valorInicial, dataCriacao);
            var dataRetirada = DateTime.UtcNow;

            var ganhos = investimento.CalcularGanhos(dataRetirada);
            var imposto = investimento.CalcularImpostos(dataRetirada);

            var impostoEsperado = Math.Round(ganhos * 0.225m, 2);
            imposto.Should().Be(impostoEsperado);
        }

        [Fact]
        public void Deve_Calcular_Imposto_18_5_Porcento_Entre_Um_E_Dois_Anos()
        {
            var investidorId = Guid.NewGuid();
            var valorInicial = 1000m;
            var dataCriacao = DateTime.UtcNow.AddMonths(-18);
            var investimento = new Investimento(investidorId, valorInicial, dataCriacao);
            var dataRetirada = DateTime.UtcNow;

            var ganhos = investimento.CalcularGanhos(dataRetirada);
            var imposto = investimento.CalcularImpostos(dataRetirada);

            var impostoEsperado = Math.Round(ganhos * 0.185m, 2);
            imposto.Should().Be(impostoEsperado);
        }

        [Fact]
        public void Deve_Calcular_Imposto_15_Porcento_Mais_De_Dois_Anos()
        {
            var investidorId = Guid.NewGuid();
            var valorInicial = 1000m;
            var dataCriacao = DateTime.UtcNow.AddMonths(-30);
            var investimento = new Investimento(investidorId, valorInicial, dataCriacao);
            var dataRetirada = DateTime.UtcNow;

            var ganhos = investimento.CalcularGanhos(dataRetirada);
            var imposto = investimento.CalcularImpostos(dataRetirada);

            var impostoEsperado = Math.Round(ganhos * 0.15m, 2);
            imposto.Should().Be(impostoEsperado);
        }
    }
}

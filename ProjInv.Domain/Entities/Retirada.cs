namespace ProjInv.Domain.Entities
{
    public class Retirada
    {
        public Guid Id { get; private set; }
        public Guid InvestimentoId { get; private set; }
        public DateTime DataRetirada { get; private set; }
        public decimal ValorBruto { get; private set; }
        public decimal Impostos { get; private set; }
        public decimal ValorLiquido { get; private set; }

        public Investimento? Investimento { get; private set; }

        private Retirada() { }

        public Retirada(Guid investimentoId, DateTime dataRetirada, decimal valorBruto, decimal impostos, decimal valorLiquido)
        {
            Id = Guid.NewGuid();
            InvestimentoId = investimentoId;
            DataRetirada = dataRetirada;
            ValorBruto = valorBruto;
            Impostos = impostos;
            ValorLiquido = valorLiquido;
        }
    }
}

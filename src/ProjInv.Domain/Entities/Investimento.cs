namespace ProjInv.Domain.Entities
{
    public class Investimento
    {
        public Guid Id { get; private set; }
        public Guid InvestidorId { get; private set; }
        public decimal ValorInicial { get; private set; }
        public DateTime DataCriacao { get; private set; }
        
        public Investidor? Investidor { get; private set; }
        public Retirada? Retirada { get; private set; }

        private Investimento() { }

        public Investimento(Guid investidorId, decimal valorInicial, DateTime dataCriacao)
        {
            if (valorInicial <= 0)
                throw new ArgumentException("O valor inicial do investimento deve ser maior que zero.", nameof(valorInicial));

            var dataCriacaoUtc = NormalizeToUtcDate(dataCriacao);

            if (dataCriacaoUtc > DateTime.UtcNow.Date)
                throw new ArgumentException("A data de criacao do investimento nao pode ser no futuro.", nameof(dataCriacao));

            Id = Guid.NewGuid();
            InvestidorId = investidorId;
            ValorInicial = valorInicial;
            DataCriacao = dataCriacaoUtc;
        }

        public bool FoiRetirado => Retirada != null;

        public decimal CalcularGanhos(DateTime? dataReferencia = null)
        {
            var dataFinal = dataReferencia.HasValue
                ? NormalizeToUtcDate(dataReferencia.Value)
                : DateTime.UtcNow.Date;
            
            if (dataFinal < DataCriacao)
                throw new ArgumentException("A data de referencia nao pode ser anterior a data de criacao do investimento.");

            if (FoiRetirado && Retirada!.DataRetirada < dataFinal)
                dataFinal = Retirada.DataRetirada;

            var meses = CalcularMesesEntreDatas(DataCriacao, dataFinal);

            var taxaMensal = 0.0052m;
            var montante = ValorInicial * (decimal)Math.Pow((double)(1 + taxaMensal), meses);
            var ganhos = montante - ValorInicial;

            return Math.Round(ganhos, 2);
        }

        public decimal CalcularSaldoEsperado(DateTime? dataReferencia = null)
        {
            return ValorInicial + CalcularGanhos(dataReferencia);
        }
        public decimal CalcularImpostos(DateTime dataRetirada)
        {
            var ganhos = CalcularGanhos(dataRetirada);
            var idade = dataRetirada - DataCriacao;

            decimal taxaImposto;
            if (idade.TotalDays < 365)
                taxaImposto = 0.225m;
            else if (idade.TotalDays < 730)
                taxaImposto = 0.185m;
            else
                taxaImposto = 0.15m;

            return Math.Round(ganhos * taxaImposto, 2);
        }

        private int CalcularMesesEntreDatas(DateTime dataInicio, DateTime dataFim)
        {
            var anos = dataFim.Year - dataInicio.Year;
            var meses = dataFim.Month - dataInicio.Month;
            var totalMeses = (anos * 12) + meses;

            if (dataFim.Day < dataInicio.Day)
                totalMeses--;

            return Math.Max(0, totalMeses);
        }

        private static DateTime NormalizeToUtcDate(DateTime dt)
        {
            DateTime utc;

            if (dt.Kind == DateTimeKind.Utc)
                utc = dt;
            else if (dt.Kind == DateTimeKind.Local)
                utc = dt.ToUniversalTime();
            else
                utc = DateTime.SpecifyKind(dt, DateTimeKind.Local).ToUniversalTime();

            return utc.Date;
        }
    }
}

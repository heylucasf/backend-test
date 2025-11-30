namespace ProjInv.Application.DTOs.Responses.Investimento
{
    public class InvestimentoResponseDto
    {
        public Guid Id { get; set; }
        public Guid InvestidorId { get; set; }
        public decimal ValorInicial { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal SaldoEsperado { get; set; }
        public bool FoiRetirado { get; set; }
        public RetiradaDto? Retirada { get; set; }
    }
}

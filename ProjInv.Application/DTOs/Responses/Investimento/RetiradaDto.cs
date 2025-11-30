namespace ProjInv.Application.DTOs.Responses.Investimento
{
    public class RetiradaDto
    {
        public Guid Id { get; set; }
        public DateTime DataRetirada { get; set; }
        public decimal ValorBruto { get; set; }
        public decimal Impostos { get; set; }
        public decimal ValorLiquido { get; set; }
    }
}

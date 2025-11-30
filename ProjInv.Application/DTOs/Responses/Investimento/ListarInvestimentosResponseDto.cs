namespace ProjInv.Application.DTOs.Responses.Investimento
{
    public class ListarInvestimentosResponseDto
    {
        public IEnumerable<InvestimentoResponseDto> Investimentos { get; set; } = new List<InvestimentoResponseDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

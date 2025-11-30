namespace ProjInv.Domain.Entities
{
    public class Investidor
    {
        public Guid Id { get; private set; }
        public string Nome { get; set; } = string.Empty;
        private readonly List<Investimento> _investimentos = new();
        public IReadOnlyCollection<Investimento> Investimentos => _investimentos.AsReadOnly();
        
        private Investidor() { }
        
        public Investidor(string nome)
        {
            Id = Guid.NewGuid();
            Nome = nome;
        }

        public void UpdateNome(string nome)
        {
            Nome = nome;
        }
    }
}

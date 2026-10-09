namespace TrabalhoPratico.Models
{
    public class Aluguel
    {

        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int VeiculoId { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public int QuilometragemInicial { get; set; }

        public int QuilometragemFinal { get; set; }

        public decimal ValorDiaria { get; set; }

        public decimal ValorTotal { get; set; }

        public Cliente? Cliente { get; set; } = null!;

        public Veiculo? Veiculo { get; set; } = null!;

        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}

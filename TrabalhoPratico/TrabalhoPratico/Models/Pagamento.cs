namespace TrabalhoPratico.Models
{
    public class Pagamento
    {
        public int Id { get; set; }

        public int AluguelId { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataPagamento { get; set; }

        public string FormaPagamento { get; set; } = null!;

        public Aluguel? Aluguel { get; set; } = null!;
    }
}

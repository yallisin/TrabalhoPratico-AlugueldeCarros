using Microsoft.EntityFrameworkCore;

namespace TrabalhoPratico.Models
{
    public class ApplicationContext : DbContext
    {

        public DbSet<Fabricante> Fabricantes { get; set; }

        public DbSet<Veiculo> Veiculos { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Aluguel> Alugueis { get; set; }

        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=SistemaVeiculos;Trusted_Connection=True;TrustServerCertificate=True;"
            );
        }

    }


}



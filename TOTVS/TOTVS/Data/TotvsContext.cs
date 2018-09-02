using Microsoft.EntityFrameworkCore;
using TOTVS.Models;

namespace TOTVS.Data
{
    public class TotvsContext : DbContext
    {
        public TotvsContext(DbContextOptions<TotvsContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ProdutoPedido> ProdutoPedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Produto>().ToTable("Produto");
            modelBuilder.Entity<Pedido>().ToTable("Pedido");
            modelBuilder.Entity<ProdutoPedido>().ToTable("ProdutoPedido");
            modelBuilder.Entity<ProdutoPedido>().HasKey(c => new { c.PedidoID, c.ProdutoID });
        }
    }
}
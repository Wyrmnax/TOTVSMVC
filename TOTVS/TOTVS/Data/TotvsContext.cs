using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Produto>().ToTable("Produto");
            modelBuilder.Entity<Pedido>().ToTable("Pedido");
        }
    }
}

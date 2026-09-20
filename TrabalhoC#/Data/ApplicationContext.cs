using Microsoft.EntityFrameworkCore;
using TrabalhoC_.Models;

namespace TrabalhoC_.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Fabricante> Fabricantes { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Veiculo> Veiculos { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Aluguel> Alugueis { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.CPF)
                .IsUnique();

            modelBuilder.Entity<Veiculo>()
                .Property(v => v.Quilometragem)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Aluguel>()
                .Property(a => a.ValorDiaria)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Aluguel>()
                .Property(a => a.ValorTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Aluguel>()
                .Property(a => a.QuilometragemInicial)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Aluguel>()
                .Property(a => a.QuilometragemFinal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Veiculo>()
                .HasOne(v => v.Fabricante)
                .WithMany(f => f.Veiculos)
                .HasForeignKey(v => v.FabricanteId);

            modelBuilder.Entity<Veiculo>()
                .HasOne(v => v.Categoria)
                .WithMany(c => c.Veiculos)
                .HasForeignKey(v => v.CategoriaId);

            modelBuilder.Entity<Aluguel>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Alugueis)
                .HasForeignKey(a => a.ClienteId);

            modelBuilder.Entity<Aluguel>()
                .HasOne(a => a.Veiculo)
                .WithMany(v => v.Alugueis)
                .HasForeignKey(a => a.VeiculoId);
        }
    }
}

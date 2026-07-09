using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Dados;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)

{
 public DbSet<Contato> Contatos { get; set; }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
  modelBuilder.Entity<Contato>().Property("nome").HasColumnName("varchar(100)");
  modelBuilder.Entity<Contato>().Property("email").HasColumnName("varchar(100)");
  modelBuilder.Entity<Contato>().Property("vinculo").HasColumnName("varchar(100)");
  modelBuilder.Entity<Contato>().Property("telefone").HasColumnName("varchar(11)");
  modelBuilder.Entity<Contato>().HasIndex("telefone").IsUnique();
  modelBuilder.Entity<Contato>().HasIndex("email").IsUnique();


 }
}
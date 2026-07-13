using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Dados;

public class AppDbContexto(DbContextOptions<AppDbContexto> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entidade =>
        {
             entidade.Property("Nome").HasColumnType("varchar").HasMaxLength(100).IsRequired();
             entidade.Property("Email").HasColumnType("varchar").HasMaxLength(100).IsRequired();
             entidade.HasIndex("Email").IsUnique();
             entidade.Property("Senha").HasColumnType("varchar").HasMaxLength(255).IsRequired();
             entidade.Property("Username").HasColumnType("varchar").HasMaxLength(30).IsRequired();
             entidade.HasIndex("Username").IsUnique();
        });
    }
}
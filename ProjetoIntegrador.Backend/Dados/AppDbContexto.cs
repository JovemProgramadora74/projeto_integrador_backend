using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Dados;

public class AppDbContexto(DbContextOptions<AppDbContexto> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().Property("Nome").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Usuario>().Property("Email").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Usuario>().HasIndex("Email").IsUnique();
        modelBuilder.Entity<Usuario>().Property("Senha").HasColumnType("varchar").HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Usuario>().Property("Username").HasColumnType("varchar").HasMaxLength(30).IsRequired();
        modelBuilder.Entity<Usuario>().HasIndex("Username").IsUnique();
    }
}
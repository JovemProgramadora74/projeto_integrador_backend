using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Dados;

public class AppDbContexto(DbContextOptions<AppDbContexto> options) : DbContext(options)
{
    public DbSet<Contato> Contatos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contato>(entity =>
        {
            entity.Property("Nome").HasColumnType("varchar(100)").IsRequired();
            entity.Property("Vinculo").HasColumnType("varchar(100)").IsRequired();
            entity.Property("Telefone").HasColumnType("varchar(11)").IsRequired();
            entity.HasIndex("Telefone").IsUnique();
            entity.Property("Email").HasColumnType("varchar(255)").IsRequired();
            entity.HasIndex("Email").IsUnique();
        });
    }
}
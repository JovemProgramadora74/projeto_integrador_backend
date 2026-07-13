using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Dados;

public class AppDbContexto(DbContextOptions<AppDbContexto> options) : DbContext(options)
{
    public DbSet<Alerta> Alertas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alerta>(entidade =>
        {
            entidade.Property("latitude").HasColumnType("decimal(10,8)").IsRequired();
            entidade.Property("longitude").HasColumnType("decimal(10,8)").IsRequired();
            entidade.Property("precisaogps").HasColumnType("decimal(10,8)").IsRequired();
            entidade.Property("status").HasColumnType("varshar(10)").IsRequired();
        });
        
    }
}
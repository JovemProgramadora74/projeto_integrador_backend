using Microsoft.EntityFrameworkCore;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Dados;

public class AppDbContexto(DbContextOptions<AppDbContexto> options) : DbContext(options)
{
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Alerta> Alertas { get; set; }

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
            entity.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId);
        });

        modelBuilder.Entity<Alerta>(entidade =>
        {
            entidade.Property("Latitude").HasPrecision(10, 8).IsRequired();
            entidade.Property("Longitude").HasPrecision(10, 8).IsRequired();
            entidade.Property("PrecisaoGps").HasPrecision(10, 8).IsRequired();
            entidade.Property("Status").HasConversion<string>().IsRequired();
            entidade.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId);
        });

        modelBuilder.Entity<Usuario>(entidade =>
        {
            entidade.Property("Nome").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            entidade.Property("Email").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            entidade.HasIndex("Email").IsUnique();
            entidade.Property("Senha").HasColumnType("varchar").HasMaxLength(100).IsRequired();
            entidade.Property("Username").HasColumnType("varchar").HasMaxLength(30).IsRequired();
            entidade.HasIndex("Username").IsUnique();
        });
    }
}
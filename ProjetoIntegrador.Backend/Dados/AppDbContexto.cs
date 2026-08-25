using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjetoIntegrador.Backend.Modelos;

namespace ProjetoIntegrador.Backend.Dados;

public class AppDbContexto(DbContextOptions<AppDbContexto> options) : DbContext(options)
{
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<Receita> Receitas { get; set; }
    public DbSet<ReceitaFavorita> ReceitasFavoritas { get; set; }

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

        modelBuilder.Entity<Receita>(entidade =>
        {
            entidade.Property(e => e.Titulo).IsRequired().HasMaxLength(150);
            entidade.Property(e => e.ImagemUrl).IsRequired().HasMaxLength(500);
            entidade.Property(e => e.TagRestricao).HasMaxLength(50).IsRequired(false);
            entidade.Property(e => e.TempoPreparoMinutos).IsRequired();
            entidade.Property(e => e.Dificuldade).IsRequired().HasMaxLength(30);
            entidade.OwnsOne(r => r.Macros, macro =>
            {
                macro.Property(m => m.ProteinaPorcentagem).HasColumnName("ProteinaPorcentagem").IsRequired();
                macro.Property(m => m.CarboidratosPorcentagem).HasColumnName("CarboidratosPorcentagem").IsRequired();
                macro.Property(m => m.GordurasPorcentagem).HasColumnName("GordurasPorcentagem").IsRequired();
            });
            entidade.Property(e => e.Ingredientes).HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<string>>(v,
                        (JsonSerializerOptions)null!) ?? new List<string>());
            entidade.Property(r => r.Ingredientes)
                .Metadata
                .SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));
            entidade.Property(e => e.Passos).HasColumnType("json")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<List<string>>(v,
                        (JsonSerializerOptions)null!) ?? new List<string>());
            entidade.Property(r => r.Passos)
                .Metadata
                .SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                ));
        });

        modelBuilder.Entity<ReceitaFavorita>(entidade =>
        {
            entidade.HasKey(e => new { e.UsuarioId, e.ReceitaId });

            entidade.HasOne(r => r.Usuario)
                .WithMany(u => u.ReceitasFavoritas)
                .HasForeignKey(rf => rf.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entidade.HasOne(rf => rf.Receita)
                .WithMany(r => r.FavoritadoPor)
                .HasForeignKey(rf => rf.ReceitaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
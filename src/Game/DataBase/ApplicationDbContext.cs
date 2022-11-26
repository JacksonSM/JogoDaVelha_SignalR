using Game.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Game.DataBase;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Partida> Partidas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        MapearPartida(builder.Entity<Partida>());
    }
    private static void MapearPartida(EntityTypeBuilder<Partida> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(c => c.JogadorLocal, jogadorLocal =>
        {
            jogadorLocal.Property(c => c.Nome)
                .IsRequired()
                .HasColumnName("JogadorLocal_Nome");

            jogadorLocal.Property(c => c.ConnectionId)
                .IsRequired()
                .HasColumnName("JogadorLocal_ConnectionIdd");
        });

        builder.OwnsOne(c => c.JogadorFora, jogadorFora =>
        {
            jogadorFora.Property(c => c.Nome)
                .HasColumnName("JogadorFora_Nome");

            jogadorFora.Property(c => c.ConnectionId)
                .HasColumnName("JogadorFora_ConnectionIdd");
        });

        builder.OwnsOne(c => c.Tabuleiro, tabuleiro =>
        {
            tabuleiro.Property(c => c.Posicoes)
                .HasColumnName("Tabuleiro_Posicoes");
        });

    }
}

using Microsoft.EntityFrameworkCore;
using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Data
{
    public class TicTacToeContext : DbContext
    {
        public TicTacToeContext(DbContextOptions<TicTacToeContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Move> Moves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.FirstPlayer)
                .WithMany()
                .HasForeignKey(g => g.FirstPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.SecondPlayer)
                .WithMany()
                .HasForeignKey(g => g.SecondPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.Moves)
                .WithOne()
                .HasForeignKey(m => m.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

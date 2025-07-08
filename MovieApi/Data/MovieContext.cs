using Microsoft.EntityFrameworkCore;
using MovieApi.Models.Entities;

namespace MovieApi.Data
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieDetails> MovieDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Actor> Actors { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1-till-1 mellan Movie och MovieDetails
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.MovieDetails)
                .WithOne(md => md.Movie)
                .HasForeignKey<MovieDetails>(md => md.MovieId);

            // Sätt sammansatt nyckel på MovieActor
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            // Definiera relationer i MovieActor (frivilligt men rekommenderat)
            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId);
        }
    }
}

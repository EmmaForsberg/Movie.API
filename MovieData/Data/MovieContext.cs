using Microsoft.EntityFrameworkCore;
using MovieApi.Data.Configurations;
using MovieCore.Entities;

namespace MovieData.Data
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
            modelBuilder.ApplyConfiguration(new ActorConfigurations());
            modelBuilder.ApplyConfiguration(new GenreConfigurations());
            modelBuilder.ApplyConfiguration(new MovieActorConfigurations());
            modelBuilder.ApplyConfiguration(new MovieConfigurations());
            modelBuilder.ApplyConfiguration(new MovieDetailsConfigurations());
            modelBuilder.ApplyConfiguration(new ReviewConfigurations());

            base.OnModelCreating(modelBuilder);
        }
    }
}

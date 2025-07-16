using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCore.Entities;

namespace MovieData.Configurations
{
    public class MovieConfigurations : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movie");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.Year)
                .IsRequired();

            builder.Property(m => m.Duration)
                .IsRequired();

            builder.HasOne(m => m.Genre)
          .WithMany(g => g.Movies)
          .HasForeignKey(m => m.GenreId)
          .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.MovieDetails)
       .WithOne(md => md.Movie)
       .HasForeignKey<MovieDetails>(md => md.MovieId)
       .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.Reviews)
       .WithOne(r => r.Movie)
       .HasForeignKey(r => r.MovieId)
       .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.MovieActors)
       .WithOne(ma => ma.Movie)
       .HasForeignKey(ma => ma.MovieId)
       .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

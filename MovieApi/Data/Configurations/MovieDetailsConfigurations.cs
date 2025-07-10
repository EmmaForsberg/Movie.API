using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApi.Models.Entities;

namespace MovieApi.Data.Configurations
{
    public class MovieDetailsConfigurations : IEntityTypeConfiguration<MovieDetails>
    {
        public void Configure(EntityTypeBuilder<MovieDetails> builder)
        {
            builder.ToTable("MovieDetails");

            // Primärnyckel och FK på samma property
            builder.HasKey(md => md.MovieId);

            builder.Property(md => md.Synopsis)
                .IsRequired()
                .HasMaxLength(500);  // justera efter behov

            builder.Property(md => md.Language)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(md => md.Budget)
                .IsRequired();

            // 1-1 relation med Movie
            builder.HasOne(md => md.Movie)
                   .WithOne(m => m.MovieDetails)
                   .HasForeignKey<MovieDetails>(md => md.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

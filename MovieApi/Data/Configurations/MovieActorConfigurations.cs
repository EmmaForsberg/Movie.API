using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieApi.Models.Entities;

namespace MovieApi.Data.Configurations
{
    public class MovieActorConfigurations : IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            builder.ToTable("MovieActor");

            // Sammansatt primärnyckel
            builder.HasKey(ma => new { ma.MovieId, ma.ActorId });

            // Relation till Movie
            builder.HasOne(ma => ma.Movie)
                   .WithMany(m => m.MovieActors)
                   .HasForeignKey(ma => ma.MovieId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relation till Actor
            builder.HasOne(ma => ma.Actor)
                   .WithMany(a => a.MovieActors)
                   .HasForeignKey(ma => ma.ActorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Property Role
            builder.Property(ma => ma.Role)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}

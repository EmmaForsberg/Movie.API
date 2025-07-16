using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCore.Entities;

namespace MovieData.Configurations
{
    public class ActorConfigurations : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.ToTable("Actor");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.BirthYear)
                .IsRequired();

            builder.HasMany(a => a.MovieActors)
    .WithOne(ma => ma.Actor)
    .HasForeignKey(ma => ma.ActorId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

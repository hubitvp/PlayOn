using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="CourtSport"/> mapping entity.
    /// </summary>
    public class CourtSportConfiguration : IEntityTypeConfiguration<CourtSport>
    {
        /// <summary>
        /// Configures the <see cref="CourtSport"/> entity: defines the composite primary key (court and sport) and the relationships to <see cref="Court"/> and <see cref="Sport"/>, using restricted delete behavior.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="CourtSport"/> entity type.</param>
        public void Configure(EntityTypeBuilder<CourtSport> builder)
        {
            builder.HasKey(x => new { x.CourtId, x.SportId });

            builder.HasOne(x => x.Court)
                .WithMany(x => x.Sports)
                .HasForeignKey(x => x.CourtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sport)
                .WithMany(x => x.Courts)
                .HasForeignKey(x => x.SportId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}

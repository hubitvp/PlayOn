using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Event"/> entity.
    /// </summary>
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        /// <summary>
        /// Configures the <see cref="Event"/> entity: defines the composite primary key (match, sport, court and venue), the one-to-one relationships with <see cref="Match"/> and <see cref="Venue"/>, the relationships with <see cref="Sport"/>, <see cref="Court"/> and <see cref="CourtSport"/>, all with restricted delete behavior.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="Event"/> entity type.</param>
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasKey(x => new
                {
                    x.MatchId,
                    x.SportId,
                    x.CourtId,
                    x.VenueId
                });

            builder
                .HasOne(x => x.Match)
                .WithOne(x => x.Event)
                .HasForeignKey<Event>(x => x.MatchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Sport)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.SportId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Court)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.CourtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.Venue)
                .WithOne(x => x.Event)
                .HasForeignKey<Event>(x => x.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(x => x.CourtSport)
                .WithMany(x => x.Events)
                .HasForeignKey(x => new { x.CourtId, x.SportId })
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

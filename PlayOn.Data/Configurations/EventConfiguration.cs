using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
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

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
            builder.HasData(
                new Event { MatchId = 1, SportId = 1, CourtId = 1, VenueId = 1 },
                new Event { MatchId = 2, SportId = 2, CourtId = 2, VenueId = 2 },
                new Event { MatchId = 3, SportId = 3, CourtId = 3, VenueId = 3 },
                new Event { MatchId = 4, SportId = 4, CourtId = 4, VenueId = 4 },
                new Event { MatchId = 5, SportId = 5, CourtId = 5, VenueId = 5 },
                new Event { MatchId = 6, SportId = 6, CourtId = 6, VenueId = 6 },
                new Event { MatchId = 7, SportId = 7, CourtId = 7, VenueId = 7 },
                new Event { MatchId = 8, SportId = 8, CourtId = 8, VenueId = 8 },
                new Event { MatchId = 9, SportId = 9, CourtId = 9, VenueId = 9 },
                new Event { MatchId = 10, SportId = 10, CourtId = 10, VenueId = 10 });
        }
    }
}

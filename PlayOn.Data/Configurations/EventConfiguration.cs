using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Configurations.SeedData;
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
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    SportId = Guid.Parse(SeedSportIds.Football),
                    CourtId = Guid.Parse(SeedCourtIds.MainFootballPitch),
                    VenueId = Guid.Parse(SeedVenueIds.Venue001)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match002),
                    SportId = Guid.Parse(SeedSportIds.Basketball),
                    CourtId = Guid.Parse(SeedCourtIds.ArenaBasketball1),
                    VenueId = Guid.Parse(SeedVenueIds.Venue002)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match003),
                    SportId = Guid.Parse(SeedSportIds.Tennis),
                    CourtId = Guid.Parse(SeedCourtIds.RiversideTennis1),
                    VenueId = Guid.Parse(SeedVenueIds.Venue003)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    SportId = Guid.Parse(SeedSportIds.Volleyball),
                    CourtId = Guid.Parse(SeedCourtIds.SouthVolleyballCourt),
                    VenueId = Guid.Parse(SeedVenueIds.Venue004)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match005),
                    SportId = Guid.Parse(SeedSportIds.Padel),
                    CourtId = Guid.Parse(SeedCourtIds.MountainPadel1),
                    VenueId = Guid.Parse(SeedVenueIds.Venue005)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match006),
                    SportId = Guid.Parse(SeedSportIds.Badminton),
                    CourtId = Guid.Parse(SeedCourtIds.WestBadminton1),
                    VenueId = Guid.Parse(SeedVenueIds.Venue006)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match007),
                    SportId = Guid.Parse(SeedSportIds.Handball),
                    CourtId = Guid.Parse(SeedCourtIds.SeaHandballCourt),
                    VenueId = Guid.Parse(SeedVenueIds.Venue007)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match008),
                    SportId = Guid.Parse(SeedSportIds.Rugby),
                    CourtId = Guid.Parse(SeedCourtIds.MaritsaRugbyField),
                    VenueId = Guid.Parse(SeedVenueIds.Venue008)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    SportId = Guid.Parse(SeedSportIds.Hockey),
                    CourtId = Guid.Parse(SeedCourtIds.GreenHockeyField),
                    VenueId = Guid.Parse(SeedVenueIds.Venue009)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match010),
                    SportId = Guid.Parse(SeedSportIds.Baseball),
                    CourtId = Guid.Parse(SeedCourtIds.DanubeBaseballField),
                    VenueId = Guid.Parse(SeedVenueIds.Venue010)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match011),
                    SportId = Guid.Parse(SeedSportIds.Football),
                    CourtId = Guid.Parse(SeedCourtIds.MainFootballPitch),
                    VenueId = Guid.Parse(SeedVenueIds.Venue011)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    SportId = Guid.Parse(SeedSportIds.Basketball),
                    CourtId = Guid.Parse(SeedCourtIds.ArenaBasketball1),
                    VenueId = Guid.Parse(SeedVenueIds.Venue012)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match013),
                    SportId = Guid.Parse(SeedSportIds.Volleyball),
                    CourtId = Guid.Parse(SeedCourtIds.ArenaBasketball1),
                    VenueId = Guid.Parse(SeedVenueIds.Venue013)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match014),
                    SportId = Guid.Parse(SeedSportIds.Padel),
                    CourtId = Guid.Parse(SeedCourtIds.MountainPadel1),
                    VenueId = Guid.Parse(SeedVenueIds.Venue014)
                },
                new Event
                {
                    MatchId = Guid.Parse(SeedMatchIds.Match015),
                    SportId = Guid.Parse(SeedSportIds.Tennis),
                    CourtId = Guid.Parse(SeedCourtIds.RiversideTennis1),
                    VenueId = Guid.Parse(SeedVenueIds.Venue015)
                });
        }
    }
}

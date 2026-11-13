using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Configurations.SeedData;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class CourtSportConfiguration : IEntityTypeConfiguration<CourtSport>
    {
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

            builder.HasData(
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.MainFootballPitch),
                    SportId = Guid.Parse(SeedSportIds.Football)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.ArenaBasketball1),
                    SportId = Guid.Parse(SeedSportIds.Basketball)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.RiversideTennis1),
                    SportId = Guid.Parse(SeedSportIds.Tennis)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.SouthVolleyballCourt),
                    SportId = Guid.Parse(SeedSportIds.Volleyball)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.MountainPadel1),
                    SportId = Guid.Parse(SeedSportIds.Padel)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.WestBadminton1),
                    SportId = Guid.Parse(SeedSportIds.Badminton)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.SeaHandballCourt),
                    SportId = Guid.Parse(SeedSportIds.Handball)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.MaritsaRugbyField),
                    SportId = Guid.Parse(SeedSportIds.Rugby)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.GreenHockeyField),
                    SportId = Guid.Parse(SeedSportIds.Hockey)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.DanubeBaseballField),
                    SportId = Guid.Parse(SeedSportIds.Baseball)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.ArenaBasketball1),
                    SportId = Guid.Parse(SeedSportIds.Volleyball)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.ArenaBasketball1),
                    SportId = Guid.Parse(SeedSportIds.Handball)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.SeaHandballCourt),
                    SportId = Guid.Parse(SeedSportIds.Basketball)
                },
                new CourtSport
                {
                    CourtId = Guid.Parse(SeedCourtIds.MainFootballPitch),
                    SportId = Guid.Parse(SeedSportIds.Hockey)
                });
        }
    }
}

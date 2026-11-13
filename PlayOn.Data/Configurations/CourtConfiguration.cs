using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.EntityConstants;
using PlayOn.Common.Enums;
using PlayOn.Data.Configurations.SeedData;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class CourtConfiguration : IEntityTypeConfiguration<Court>
    {
        public void Configure(EntityTypeBuilder<Court> builder)
        {
            builder.Property(x => x.HourlyPrice)
                .HasPrecision(
                    CourtConstants.DecimalPrecision,
                    CourtConstants.DecimalScale);

            builder.HasData(
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.MainFootballPitch),
                    Name = "Main Football Pitch",
                    Surface = Surface.Artificial,
                    Capacity = 22,
                    HourlyPrice = 80m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.ArenaBasketball1),
                    Name = "Arena Basketball 1",
                    Surface = Surface.Hardwood,
                    Capacity = 10,
                    HourlyPrice = 45m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.RiversideTennis1),
                    Name = "Riverside Tennis 1",
                    Surface = Surface.Clay,
                    Capacity = 4,
                    HourlyPrice = 25m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.SouthVolleyballCourt),
                    Name = "South Volleyball Court",
                    Surface = Surface.Sand,
                    Capacity = 12,
                    HourlyPrice = 35m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.MountainPadel1),
                    Name = "Mountain Padel 1",
                    Surface = Surface.Artificial,
                    Capacity = 4,
                    HourlyPrice = 30m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.WestBadminton1),
                    Name = "West Badminton 1",
                    Surface = Surface.Hardwood,
                    Capacity = 4,
                    HourlyPrice = 20m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.SeaHandballCourt),
                    Name = "Sea Handball Court",
                    Surface = Surface.Concrete,
                    Capacity = 14,
                    HourlyPrice = 50m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.MaritsaRugbyField),
                    Name = "Maritsa Rugby Field",
                    Surface = Surface.Grass,
                    Capacity = 30,
                    HourlyPrice = 90m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.GreenHockeyField),
                    Name = "Green Hockey Field",
                    Surface = Surface.Artificial,
                    Capacity = 22,
                    HourlyPrice = 75m,
                },
                new Court
                {
                    Id = Guid.Parse(SeedCourtIds.DanubeBaseballField),
                    Name = "Danube Baseball Field",
                    Surface = Surface.Grass,
                    Capacity = 18,
                    HourlyPrice = 65m,
                });
        }
    }
}

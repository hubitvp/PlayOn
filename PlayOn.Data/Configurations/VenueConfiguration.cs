using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Configurations.SeedData;
using PlayOn.Data.Models;
using static PlayOn.Common.EntityConstants.VenueConstants;

namespace PlayOn.Data.Configurations
{
    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.Property(x => x.Latitude)
                .HasPrecision(DecimalPrecision, DecimalScale);

            builder.Property(x => x.Longitude)
                .HasPrecision(DecimalPrecision, DecimalScale);

            builder.HasData(
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue001),
                    Date = new DateOnly(2026, 10, 13),
                    StartsAt = new TimeOnly(18, 0),
                    Name = "Central Sports Park",
                    Address = "1 Main Street",
                    City = "Sofia",
                    Latitude = 42.697700m,
                    Longitude = 23.321900m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(7, 0),
                    ClosesAt = new TimeOnly(23, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue002),
                    Date = new DateOnly(2026, 10, 14),
                    StartsAt = new TimeOnly(19, 0),
                    Name = "Eastside Arena",
                    Address = "12 East Boulevard",
                    City = "Plovdiv",
                    Latitude = 42.135400m,
                    Longitude = 24.745300m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(22, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue003),
                    Date = new DateOnly(2026, 10, 17),
                    StartsAt = new TimeOnly(10, 0),
                    Name = "Riverside Courts",
                    Address = "8 River Road",
                    City = "Varna",
                    Latitude = 43.214100m,
                    Longitude = 27.914700m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(6, 30),
                    ClosesAt = new TimeOnly(22, 30),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue004),
                    Date = new DateOnly(2026, 10, 18),
                    StartsAt = new TimeOnly(9, 0),
                    Name = "South Field Complex",
                    Address = "24 South Park",
                    City = "Burgas",
                    Latitude = 42.504800m,
                    Longitude = 27.462600m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(7, 30),
                    ClosesAt = new TimeOnly(21, 30),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue005),
                    Date = new DateOnly(2026, 11, 9),
                    StartsAt = new TimeOnly(18, 30),
                    Name = "Mountain Sports Hall",
                    Address = "5 Vitosha Avenue",
                    City = "Sofia",
                    Latitude = 42.650700m,
                    Longitude = 23.347000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(23, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue006),
                    Date = new DateOnly(2026, 11, 10),
                    StartsAt = new TimeOnly(17, 0),
                    Name = "West End Club",
                    Address = "44 West Street",
                    City = "Sofia",
                    Latitude = 42.705000m,
                    Longitude = 23.270000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(9, 0),
                    ClosesAt = new TimeOnly(22, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue007),
                    Date = new DateOnly(2026, 11, 11),
                    StartsAt = new TimeOnly(19, 30),
                    Name = "Black Sea Sports",
                    Address = "19 Coast Lane",
                    City = "Varna",
                    Latitude = 43.207000m,
                    Longitude = 27.922000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(7, 0),
                    ClosesAt = new TimeOnly(21, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue008),
                    Date = new DateOnly(2026, 11, 14),
                    StartsAt = new TimeOnly(11, 0),
                    Name = "Maritsa Activity Center",
                    Address = "31 Maritsa Road",
                    City = "Plovdiv",
                    Latitude = 42.145000m,
                    Longitude = 24.760000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(22, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue009),
                    Date = new DateOnly(2026, 10, 25),
                    StartsAt = new TimeOnly(14, 0),
                    Name = "Green Court Club",
                    Address = "7 Garden Street",
                    City = "Ruse",
                    Latitude = 43.835000m,
                    Longitude = 25.965000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(7, 30),
                    ClosesAt = new TimeOnly(21, 30),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue010),
                    Date = new DateOnly(2026, 11, 15),
                    StartsAt = new TimeOnly(15, 0),
                    Name = "Danube Sports Hub",
                    Address = "16 Harbor Avenue",
                    City = "Ruse",
                    Latitude = 43.848000m,
                    Longitude = 25.960000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 30),
                    ClosesAt = new TimeOnly(22, 30),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue011),
                    Date = new DateOnly(2026, 11, 7),
                    StartsAt = new TimeOnly(10, 0),
                    Name = "Central Sports Park",
                    Address = "1 Main Street",
                    City = "Sofia",
                    Latitude = 42.697700m,
                    Longitude = 23.321900m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(7, 0),
                    ClosesAt = new TimeOnly(23, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue012),
                    Date = new DateOnly(2026, 11, 13),
                    StartsAt = new TimeOnly(20, 0),
                    Name = "Eastside Arena",
                    Address = "12 East Boulevard",
                    City = "Plovdiv",
                    Latitude = 42.135400m,
                    Longitude = 24.745300m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(22, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue013),
                    Date = new DateOnly(2026, 11, 16),
                    StartsAt = new TimeOnly(18, 30),
                    Name = "Eastside Arena",
                    Address = "12 East Boulevard",
                    City = "Plovdiv",
                    Latitude = 42.135400m,
                    Longitude = 24.745300m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(22, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue014),
                    Date = new DateOnly(2026, 11, 18),
                    StartsAt = new TimeOnly(19, 0),
                    Name = "Mountain Sports Hall",
                    Address = "5 Vitosha Avenue",
                    City = "Sofia",
                    Latitude = 42.650700m,
                    Longitude = 23.347000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(23, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue015),
                    Date = new DateOnly(2026, 11, 22),
                    StartsAt = new TimeOnly(9, 30),
                    Name = "Riverside Courts",
                    Address = "8 River Road",
                    City = "Varna",
                    Latitude = 43.214100m,
                    Longitude = 27.914700m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(6, 30),
                    ClosesAt = new TimeOnly(22, 30),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue016),
                    Date = new DateOnly(2026, 11, 20),
                    StartsAt = new TimeOnly(18, 0),
                    Name = "Central Sports Park",
                    Address = "1 Main Street",
                    City = "Sofia",
                    Latitude = 42.697700m,
                    Longitude = 23.321900m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(7, 0),
                    ClosesAt = new TimeOnly(23, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue017),
                    Date = new DateOnly(2026, 11, 21),
                    StartsAt = new TimeOnly(10, 0),
                    Name = "Eastside Arena",
                    Address = "12 East Boulevard",
                    City = "Plovdiv",
                    Latitude = 42.135400m,
                    Longitude = 24.745300m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(22, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue018),
                    Date = new DateOnly(2026, 11, 24),
                    StartsAt = new TimeOnly(19, 0),
                    Name = "Riverside Courts",
                    Address = "8 River Road",
                    City = "Varna",
                    Latitude = 43.214100m,
                    Longitude = 27.914700m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(6, 30),
                    ClosesAt = new TimeOnly(22, 30),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue019),
                    Date = new DateOnly(2026, 11, 27),
                    StartsAt = new TimeOnly(17, 30),
                    Name = "South Field Complex",
                    Address = "24 South Park",
                    City = "Burgas",
                    Latitude = 42.504800m,
                    Longitude = 27.462600m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(7, 30),
                    ClosesAt = new TimeOnly(21, 30),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue020),
                    Date = new DateOnly(2026, 11, 28),
                    StartsAt = new TimeOnly(11, 0),
                    Name = "Mountain Sports Hall",
                    Address = "5 Vitosha Avenue",
                    City = "Sofia",
                    Latitude = 42.650700m,
                    Longitude = 23.347000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 0),
                    ClosesAt = new TimeOnly(23, 0),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue021),
                    Date = new DateOnly(2026, 12, 3),
                    StartsAt = new TimeOnly(18, 0),
                    Name = "Danube Sports Hub",
                    Address = "16 Harbor Avenue",
                    City = "Ruse",
                    Latitude = 43.848000m,
                    Longitude = 25.960000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(8, 30),
                    ClosesAt = new TimeOnly(22, 30),
                    IsDeleted = false
                },
                new Venue
                {
                    Id = Guid.Parse(SeedVenueIds.Venue022),
                    Date = new DateOnly(2026, 12, 5),
                    StartsAt = new TimeOnly(15, 0),
                    Name = "West End Club",
                    Address = "44 West Street",
                    City = "Sofia",
                    Latitude = 42.705000m,
                    Longitude = 23.270000m,
                    PhotoUrl = null,
                    OpensAt = new TimeOnly(9, 0),
                    ClosesAt = new TimeOnly(22, 0),
                    IsDeleted = false
                }
            );
        }
    }
}

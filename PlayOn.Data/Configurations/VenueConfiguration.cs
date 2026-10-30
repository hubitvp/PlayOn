using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
                new Venue { Id = 1, Name = "Central Sports Park", Address = "1 Main Street", City = "Sofia", Latitude = 42.697700m, Longitude = 23.321900m, OpensAt = new TimeOnly(7, 0), ClosesAt = new TimeOnly(23, 0), PhotoUrl = null, IsDeleted = false },
                new Venue { Id = 2, Name = "Eastside Arena", Address = "12 East Boulevard", City = "Plovdiv", Latitude = 42.135400m, Longitude = 24.745300m, PhotoUrl = null, OpensAt = new TimeOnly(8, 0), ClosesAt = new TimeOnly(22, 0), IsDeleted = false },
                new Venue { Id = 3, Name = "Riverside Courts", Address = "8 River Road", City = "Varna", Latitude = 43.214100m, Longitude = 27.914700m, PhotoUrl = null, OpensAt = new TimeOnly(6, 30), ClosesAt = new TimeOnly(22, 30), IsDeleted = false },
                new Venue { Id = 4, Name = "South Field Complex", Address = "24 South Park", City = "Burgas", Latitude = 42.504800m, Longitude = 27.462600m, PhotoUrl = null, OpensAt = new TimeOnly(7, 30), ClosesAt = new TimeOnly(21, 30), IsDeleted = false },
                new Venue { Id = 5, Name = "Mountain Sports Hall", Address = "5 Vitosha Avenue", City = "Sofia", Latitude = 42.650700m, Longitude = 23.347000m, PhotoUrl = null, OpensAt = new TimeOnly(8, 0), ClosesAt = new TimeOnly(23, 0), IsDeleted = false },
                new Venue { Id = 6, Name = "West End Club", Address = "44 West Street", City = "Sofia", Latitude = 42.705000m, Longitude = 23.270000m, PhotoUrl = null, OpensAt = new TimeOnly(9, 0), ClosesAt = new TimeOnly(22, 0), IsDeleted = false },
                new Venue { Id = 7, Name = "Black Sea Sports", Address = "19 Coast Lane", City = "Varna", Latitude = 43.207000m, Longitude = 27.922000m, PhotoUrl = null, OpensAt = new TimeOnly(7, 0), ClosesAt = new TimeOnly(21, 0), IsDeleted = false },
                new Venue { Id = 8, Name = "Maritsa Activity Center", Address = "31 Maritsa Road", City = "Plovdiv", Latitude = 42.145000m, Longitude = 24.760000m, PhotoUrl = null, OpensAt = new TimeOnly(8, 0), ClosesAt = new TimeOnly(22, 0), IsDeleted = false },
                new Venue { Id = 9, Name = "Green Court Club", Address = "7 Garden Street", City = "Ruse", Latitude = 43.835000m, Longitude = 25.965000m, PhotoUrl = null, OpensAt = new TimeOnly(7, 30), ClosesAt = new TimeOnly(21, 30), IsDeleted = false },
                new Venue { Id = 10, Name = "Danube Sports Hub", Address = "16 Harbor Avenue", City = "Ruse", Latitude = 43.848000m, Longitude = 25.960000m, PhotoUrl = null, OpensAt = new TimeOnly(8, 30), ClosesAt = new TimeOnly(22, 30), IsDeleted = false });
        }
    }
}

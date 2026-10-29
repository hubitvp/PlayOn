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

        }
    }
}

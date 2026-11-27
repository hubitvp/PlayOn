using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;
using static PlayOn.Common.EntityConstants.VenueConstants;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Venue"/> entity.
    /// </summary>
    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        /// <summary>
        /// Configures the <see cref="Venue"/> entity: sets the decimal precision and scale of the latitude and longitude coordinates.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="Venue"/> entity type.</param>
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.Property(x => x.Latitude)
                .HasPrecision(DecimalPrecision, DecimalScale);

            builder.Property(x => x.Longitude)
                .HasPrecision(DecimalPrecision, DecimalScale);

        }
    }
}

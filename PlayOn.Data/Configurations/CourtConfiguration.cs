using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.EntityConstants;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Court"/> entity.
    /// </summary>
    public class CourtConfiguration : IEntityTypeConfiguration<Court>
    {
        /// <summary>
        /// Configures the <see cref="Court"/> entity: sets the decimal precision and scale of the hourly price.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="Court"/> entity type.</param>
        public void Configure(EntityTypeBuilder<Court> builder)
        {
            builder.Property(x => x.HourlyPrice)
                .HasPrecision(
                    CourtConstants.DecimalPrecision,
                    CourtConstants.DecimalScale);

        }
    }
}

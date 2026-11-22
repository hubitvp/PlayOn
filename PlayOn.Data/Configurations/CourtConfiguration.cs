using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.EntityConstants;
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

        }
    }
}

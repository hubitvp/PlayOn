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


            builder.HasOne(x => x.Venue)
                .WithMany(x => x.Courts)
                .HasForeignKey(x => x.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sport)
                .WithMany(x => x.Courts)
                .HasForeignKey(x => x.SportId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

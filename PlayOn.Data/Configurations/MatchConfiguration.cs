using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;
using static PlayOn.Common.EntityConstants.MatchConstants;


namespace PlayOn.Data.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {


            builder.Property(x => x.PricePerPlayer)
                .HasPrecision(DecimalPrecision, DecimalScale);


            builder.Property(x => x.RowVersion)
                .IsRowVersion();


            builder.HasOne(x => x.Organiser)
                .WithMany()
                .HasForeignKey(x => x.OrganiserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Court)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.CourtId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}

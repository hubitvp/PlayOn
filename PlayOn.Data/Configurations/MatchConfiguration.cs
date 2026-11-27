using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;
using static PlayOn.Common.EntityConstants.MatchConstants;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="Match"/> entity.
    /// </summary>
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        /// <summary>
        /// Configures the <see cref="Match"/> entity: sets the decimal precision of the price per player, marks <c>RowVersion</c> as a concurrency token, and defines the relationship to the organiser with restricted delete behavior.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="Match"/> entity type.</param>
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

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    /// <summary>
    /// Entity Framework Core configuration for the <see cref="PlayerRating"/> entity.
    /// </summary>
    public class PlayerRatingConfiguration : IEntityTypeConfiguration<PlayerRating>
    {
        /// <summary>
        /// Configures the <see cref="PlayerRating"/> entity: defines the composite primary key (match, rater and rated user) and the relationships to <see cref="Match"/> and to both the rating and the rated <see cref="ApplicationUser"/>, using restricted delete behavior.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="PlayerRating"/> entity type.</param>
        public void Configure(EntityTypeBuilder<PlayerRating> builder)
        {

            builder.HasKey(x => new
            {
                x.MatchId,
                x.RaterId,
                x.RatedUserId
            });

            builder.HasOne(x => x.Match)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Rater)
                .WithMany()
                .HasForeignKey(x => x.RaterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RatedUser)
                .WithMany()
                .HasForeignKey(x => x.RatedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

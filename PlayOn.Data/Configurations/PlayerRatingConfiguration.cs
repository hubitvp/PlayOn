using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class PlayerRatingConfiguration : IEntityTypeConfiguration<PlayerRating>
    {
        public void Configure(EntityTypeBuilder<PlayerRating> builder)
        {

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

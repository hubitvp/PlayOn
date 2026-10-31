using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Data.Configurations.SeedData;
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


            builder.HasData(
                new PlayerRating { Id = 1, MatchId = 1, RaterId = SeedUserIds.User001, RatedUserId = SeedUserIds.User002, Score = 5, Comment = "Superb teammate, great energy.", IsHidden = false },
                new PlayerRating { Id = 2, MatchId = 1, RaterId = SeedUserIds.User002, RatedUserId = SeedUserIds.User003, Score = 3, Comment = "Decent game, nothing special.", IsHidden = false },
                new PlayerRating { Id = 3, MatchId = 1, RaterId = SeedUserIds.User003, RatedUserId = SeedUserIds.User004, Score = 3, Comment = "Decent game, nothing special.", IsHidden = false },
                new PlayerRating { Id = 4, MatchId = 2, RaterId = SeedUserIds.User002, RatedUserId = SeedUserIds.User001, Score = 3, Comment = "Decent game, nothing special.", IsHidden = false },
                new PlayerRating { Id = 5, MatchId = 2, RaterId = SeedUserIds.User001, RatedUserId = SeedUserIds.User004, Score = 3, Comment = "Decent game, nothing special.", IsHidden = false },
                new PlayerRating { Id = 6, MatchId = 3, RaterId = SeedUserIds.User003, RatedUserId = SeedUserIds.User009, Score = 5, Comment = "Superb teammate, great energy.", IsHidden = false },
                new PlayerRating { Id = 7, MatchId = 3, RaterId = SeedUserIds.User009, RatedUserId = SeedUserIds.User010, Score = 4, Comment = null, IsHidden = false },
                new PlayerRating { Id = 8, MatchId = 4, RaterId = SeedUserIds.User004, RatedUserId = SeedUserIds.User003, Score = 4, Comment = null, IsHidden = false },
                new PlayerRating { Id = 9, MatchId = 4, RaterId = SeedUserIds.User003, RatedUserId = SeedUserIds.User005, Score = 5, Comment = "Superb teammate, great energy.", IsHidden = false },
                new PlayerRating { Id = 10, MatchId = 9, RaterId = SeedUserIds.User009, RatedUserId = SeedUserIds.User007, Score = 5, Comment = "Superb teammate, great energy.", IsHidden = false });
        }
    }
}

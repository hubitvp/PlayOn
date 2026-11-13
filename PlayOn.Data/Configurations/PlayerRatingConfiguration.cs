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
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating001),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    RaterId = Guid.Parse(SeedUserIds.User001),
                    RatedUserId = Guid.Parse(SeedUserIds.User002),
                    Score = 5,
                    Comment = "Superb teammate, great energy.",
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating002),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    RaterId = Guid.Parse(SeedUserIds.User002),
                    RatedUserId = Guid.Parse(SeedUserIds.User003),
                    Score = 3,
                    Comment = "Decent game, nothing special.",
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating003),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    RaterId = Guid.Parse(SeedUserIds.User003),
                    RatedUserId = Guid.Parse(SeedUserIds.User004),
                    Score = 3,
                    Comment = "Decent game, nothing special.",
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating004),
                    MatchId = Guid.Parse(SeedMatchIds.Match002),
                    RaterId = Guid.Parse(SeedUserIds.User002),
                    RatedUserId = Guid.Parse(SeedUserIds.User001),
                    Score = 3,
                    Comment = "Decent game, nothing special.",
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating005),
                    MatchId = Guid.Parse(SeedMatchIds.Match002),
                    RaterId = Guid.Parse(SeedUserIds.User001),
                    RatedUserId = Guid.Parse(SeedUserIds.User004),
                    Score = 3,
                    Comment = "Decent game, nothing special.",
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating006),
                    MatchId = Guid.Parse(SeedMatchIds.Match003),
                    RaterId = Guid.Parse(SeedUserIds.User003),
                    RatedUserId = Guid.Parse(SeedUserIds.User009),
                    Score = 5,
                    Comment = "Superb teammate, great energy.",
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating007),
                    MatchId = Guid.Parse(SeedMatchIds.Match003),
                    RaterId = Guid.Parse(SeedUserIds.User009),
                    RatedUserId = Guid.Parse(SeedUserIds.User010),
                    Score = 4,
                    Comment = null,
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating008),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    RaterId = Guid.Parse(SeedUserIds.User004),
                    RatedUserId = Guid.Parse(SeedUserIds.User003),
                    Score = 4,
                    Comment = null,
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating009),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    RaterId = Guid.Parse(SeedUserIds.User003),
                    RatedUserId = Guid.Parse(SeedUserIds.User005),
                    Score = 5,
                    Comment = "Superb teammate, great energy.",
                    IsHidden = false
                },
                new PlayerRating
                {
                    Id = Guid.Parse(SeedPlayerRatingIds.Rating010),
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    RaterId = Guid.Parse(SeedUserIds.User009),
                    RatedUserId = Guid.Parse(SeedUserIds.User007),
                    Score = 5,
                    Comment = "Superb teammate, great energy.",
                    IsHidden = false
                });
        }
    }
}

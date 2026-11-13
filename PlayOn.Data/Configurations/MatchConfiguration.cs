using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.Enums;
using PlayOn.Data.Configurations.SeedData;
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

            builder.HasData(
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match001),
                    OrganiserId = Guid.Parse(SeedUserIds.User001),
                    Title = "Tuesday Evening Football",
                    Description = "Casual five-a-side football.",
                    DurationMinutes = 90,
                    MinPlayers = 6,
                    MaxPlayers = 22,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 8m,
                    Status = MatchStatus.Completed,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match002),
                    OrganiserId = Guid.Parse(SeedUserIds.User002),
                    Title = "Downtown Basketball",
                    Description = "Competitive five-on-five game.",
                    DurationMinutes = 90,
                    MinPlayers = 4,
                    MaxPlayers = 10,
                    MinSkill = SkillLevel.Intermediate,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 5m,
                    Status = MatchStatus.Completed,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match003),
                    OrganiserId = Guid.Parse(SeedUserIds.User003),
                    Title = "Saturday Tennis Doubles",
                    Description = "Friendly doubles session.",
                    DurationMinutes = 120,
                    MinPlayers = 2,
                    MaxPlayers = 4,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Intermediate,
                    PricePerPlayer = 6m,
                    Status = MatchStatus.Completed,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match004),
                    OrganiserId = Guid.Parse(SeedUserIds.User004),
                    Title = "Beach Volleyball Morning",
                    Description = "Three-on-three beach volleyball.",
                    DurationMinutes = 90,
                    MinPlayers = 6,
                    MaxPlayers = 12,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 7m,
                    Status = MatchStatus.Completed,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match005),
                    OrganiserId = Guid.Parse(SeedUserIds.User005),
                    Title = "Padel After Work",
                    Description = "Fast-paced padel games.",
                    DurationMinutes = 90,
                    MinPlayers = 2,
                    MaxPlayers = 4,
                    MinSkill = SkillLevel.Intermediate,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 9m,
                    Status = MatchStatus.Confirmed,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match006),
                    OrganiserId = Guid.Parse(SeedUserIds.User006),
                    Title = "Badminton Open Play",
                    Description = "Singles and doubles rotation.",
                    DurationMinutes = 120,
                    MinPlayers = 2,
                    MaxPlayers = 4,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Intermediate,
                    PricePerPlayer = 4m,
                    Status = MatchStatus.Confirmed,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match007),
                    OrganiserId = Guid.Parse(SeedUserIds.User007),
                    Title = "Handball Skills Match",
                    Description = "A structured handball session.",
                    DurationMinutes = 90,
                    MinPlayers = 8,
                    MaxPlayers = 14,
                    MinSkill = SkillLevel.Intermediate,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 6m,
                    Status = MatchStatus.Open,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match008),
                    OrganiserId = Guid.Parse(SeedUserIds.User008),
                    Title = "Weekend Rugby Touch",
                    Description = "Non-contact touch rugby.",
                    DurationMinutes = 90,
                    MinPlayers = 10,
                    MaxPlayers = 30,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Intermediate,
                    PricePerPlayer = 8m,
                    Status = MatchStatus.Open,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match009),
                    OrganiserId = Guid.Parse(SeedUserIds.User009),
                    Title = "Hockey Training Game",
                    Description = "Small-sided field hockey.",
                    DurationMinutes = 90,
                    MinPlayers = 6,
                    MaxPlayers = 22,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 7m,
                    Status = MatchStatus.Completed,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match010),
                    OrganiserId = Guid.Parse(SeedUserIds.User010),
                    Title = "Baseball Practice Game",
                    Description = "Relaxed practice game for all levels.",
                    DurationMinutes = 120,
                    MinPlayers = 6,
                    MaxPlayers = 18,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Intermediate,
                    PricePerPlayer = 5m,
                    Status = MatchStatus.Cancelled,
                    CancelReason = "Venue maintenance",
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match011),
                    OrganiserId = Guid.Parse(SeedUserIds.User001),
                    Title = "Saturday Morning Football",
                    Description = "Friendly kickabout, all levels welcome.",
                    DurationMinutes = 90,
                    MinPlayers = 6,
                    MaxPlayers = 22,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 7m,
                    Status = MatchStatus.Open,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match012),
                    OrganiserId = Guid.Parse(SeedUserIds.User002),
                    Title = "Friday Night Basketball",
                    Description = "Full-court games, winners stay on.",
                    DurationMinutes = 90,
                    MinPlayers = 4,
                    MaxPlayers = 8,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 5m,
                    Status = MatchStatus.Confirmed,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match013),
                    OrganiserId = Guid.Parse(SeedUserIds.User004),
                    Title = "Monday Volleyball Social",
                    Description = "Relaxed indoor volleyball with mixed teams.",
                    DurationMinutes = 90,
                    MinPlayers = 6,
                    MaxPlayers = 10,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Intermediate,
                    PricePerPlayer = 6m,
                    Status = MatchStatus.Open,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match014),
                    OrganiserId = Guid.Parse(SeedUserIds.User005),
                    Title = "Padel Doubles Evening",
                    Description = "Four players, rotating partners.",
                    DurationMinutes = 90,
                    MinPlayers = 4,
                    MaxPlayers = 4,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Advanced,
                    PricePerPlayer = 9m,
                    Status = MatchStatus.Open,
                    IsHidden = false,
                    IsDeleted = false
                },
                new Match
                {
                    Id = Guid.Parse(SeedMatchIds.Match015),
                    OrganiserId = Guid.Parse(SeedUserIds.User003),
                    Title = "Sunday Tennis Singles",
                    Description = "Friendly singles on clay, bring your racket.",
                    DurationMinutes = 60,
                    MinPlayers = 2,
                    MaxPlayers = 4,
                    MinSkill = SkillLevel.Beginner,
                    MaxSkill = SkillLevel.Intermediate,
                    PricePerPlayer = 6m,
                    Status = MatchStatus.Confirmed,
                    IsHidden = false,
                    IsDeleted = false
                });
        }
    }
}

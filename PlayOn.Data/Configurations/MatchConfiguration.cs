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

            builder.HasOne(x => x.Court)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.CourtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Match { Id = 1, OrganiserId = SeedUserIds.User001, CourtId = 1, Title = "Tuesday Evening Football", Description = "Casual five-a-side football.", StartsAt = new DateTime(2026, 11, 3, 18, 0, 0), DurationMinutes = 90, MinPlayers = 10, MaxPlayers = 22, MinSkill = SkillLevel.Beginner, MaxSkill = SkillLevel.Advanced, PricePerPlayer = 8m, Status = MatchStatus.Open, IsHidden = false, IsDeleted = false },
                new Match { Id = 2, OrganiserId = SeedUserIds.User002, CourtId = 2, Title = "Downtown Basketball", Description = "Competitive five-on-five game.", StartsAt = new DateTime(2026, 11, 4, 19, 0, 0), DurationMinutes = 90, MinPlayers = 8, MaxPlayers = 10, MinSkill = SkillLevel.Intermediate, MaxSkill = SkillLevel.Advanced, PricePerPlayer = 5m, Status = MatchStatus.Confirmed, IsHidden = false, IsDeleted = false },
                new Match { Id = 3, OrganiserId = SeedUserIds.User003, CourtId = 3, Title = "Saturday Tennis Doubles", Description = "Friendly doubles session.", StartsAt = new DateTime(2026, 11, 7, 10, 0, 0), DurationMinutes = 120, MinPlayers = 2, MaxPlayers = 4, MinSkill = SkillLevel.Beginner, MaxSkill = SkillLevel.Intermediate, PricePerPlayer = 6m, Status = MatchStatus.Open, IsHidden = false, IsDeleted = false },
                new Match { Id = 4, OrganiserId = SeedUserIds.User004, CourtId = 4, Title = "Beach Volleyball Morning", Description = "Three-on-three beach volleyball.", StartsAt = new DateTime(2026, 11, 8, 9, 0, 0), DurationMinutes = 90, MinPlayers = 6, MaxPlayers = 12, MinSkill = SkillLevel.Beginner, MaxSkill = SkillLevel.Advanced, PricePerPlayer = 7m, Status = MatchStatus.Open, IsHidden = false, IsDeleted = false },
                new Match { Id = 5, OrganiserId = SeedUserIds.User005, CourtId = 5, Title = "Padel After Work", Description = "Fast-paced padel games.", StartsAt = new DateTime(2026, 11, 9, 18, 30, 0), DurationMinutes = 90, MinPlayers = 2, MaxPlayers = 4, MinSkill = SkillLevel.Intermediate, MaxSkill = SkillLevel.Advanced, PricePerPlayer = 9m, Status = MatchStatus.Confirmed, IsHidden = false, IsDeleted = false },
                new Match { Id = 6, OrganiserId = SeedUserIds.User006, CourtId = 6, Title = "Badminton Open Play", Description = "Singles and doubles rotation.", StartsAt = new DateTime(2026, 11, 10, 17, 0, 0), DurationMinutes = 120, MinPlayers = 2, MaxPlayers = 4, MinSkill = SkillLevel.Beginner, MaxSkill = SkillLevel.Intermediate, PricePerPlayer = 4m, Status = MatchStatus.Open, IsHidden = false, IsDeleted = false },
                new Match { Id = 7, OrganiserId = SeedUserIds.User007, CourtId = 7, Title = "Handball Skills Match", Description = "A structured handball session.", StartsAt = new DateTime(2026, 11, 11, 19, 30, 0), DurationMinutes = 90, MinPlayers = 8, MaxPlayers = 14, MinSkill = SkillLevel.Intermediate, MaxSkill = SkillLevel.Advanced, PricePerPlayer = 6m, Status = MatchStatus.Open, IsHidden = false, IsDeleted = false },
                new Match { Id = 8, OrganiserId = SeedUserIds.User008, CourtId = 8, Title = "Weekend Rugby Touch", Description = "Non-contact touch rugby.", StartsAt = new DateTime(2026, 11, 14, 11, 0, 0), DurationMinutes = 90, MinPlayers = 10, MaxPlayers = 30, MinSkill = SkillLevel.Beginner, MaxSkill = SkillLevel.Intermediate, PricePerPlayer = 8m, Status = MatchStatus.Open, IsHidden = false, IsDeleted = false },
                new Match { Id = 9, OrganiserId = SeedUserIds.User009, CourtId = 9, Title = "Hockey Training Game", Description = "Small-sided field hockey.", StartsAt = new DateTime(2026, 11, 15, 14, 0, 0), DurationMinutes = 90, MinPlayers = 10, MaxPlayers = 22, MinSkill = SkillLevel.Intermediate, MaxSkill = SkillLevel.Advanced, PricePerPlayer = 7m, Status = MatchStatus.Completed, IsHidden = false, IsDeleted = false },
                new Match { Id = 10, OrganiserId = SeedUserIds.User010, CourtId = 10, Title = "Baseball Practice Game", Description = "Relaxed practice game for all levels.", StartsAt = new DateTime(2026, 11, 15, 15, 0, 0), DurationMinutes = 120, MinPlayers = 6, MaxPlayers = 18, MinSkill = SkillLevel.Beginner, MaxSkill = SkillLevel.Intermediate, PricePerPlayer = 5m, Status = MatchStatus.Cancelled, CancelReason = "Venue maintenance", IsHidden = false, IsDeleted = false });

        }
    }
}

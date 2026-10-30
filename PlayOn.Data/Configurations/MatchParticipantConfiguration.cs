using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.Enums;
using PlayOn.Data.Configurations.SeedData;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class MatchParticipantConfiguration : IEntityTypeConfiguration<MatchParticipant>
    {
        public void Configure(EntityTypeBuilder<MatchParticipant> builder)
        {
            builder.HasOne(x => x.Match)
                .WithMany(x => x.Participants)
                .HasForeignKey(x => x.MatchId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasData(
                new MatchParticipant { Id = 1, MatchId = 1, UserId = SeedUserIds.User001, Status = ParticipantStatus.Joined, JoinedOn = new DateTime(2026, 10, 20), StatusChangedOn = new DateTime(2026, 10, 20), IsLateCancel = false },
                new MatchParticipant { Id = 2, MatchId = 1, UserId = SeedUserIds.User002, Status = ParticipantStatus.Joined, JoinedOn = new DateTime(2026, 10, 21), StatusChangedOn = new DateTime(2026, 10, 21), IsLateCancel = false },
                new MatchParticipant { Id = 3, MatchId = 2, UserId = SeedUserIds.User002, Status = ParticipantStatus.Attended, JoinedOn = new DateTime(2026, 10, 15), StatusChangedOn = new DateTime(2026, 11, 4), IsLateCancel = false },
                new MatchParticipant { Id = 4, MatchId = 2, UserId = SeedUserIds.User003, Status = ParticipantStatus.Attended, JoinedOn = new DateTime(2026, 10, 16), StatusChangedOn = new DateTime(2026, 11, 4), IsLateCancel = false },
                new MatchParticipant { Id = 5, MatchId = 3, UserId = SeedUserIds.User004, Status = ParticipantStatus.Joined, JoinedOn = new DateTime(2026, 10, 22), StatusChangedOn = new DateTime(2026, 10, 22), IsLateCancel = false },
                new MatchParticipant { Id = 6, MatchId = 4, UserId = SeedUserIds.User005, Status = ParticipantStatus.Waitlisted, JoinedOn = new DateTime(2026, 10, 23), StatusChangedOn = new DateTime(2026, 10, 23), IsLateCancel = false },
                new MatchParticipant { Id = 7, MatchId = 5, UserId = SeedUserIds.User006, Status = ParticipantStatus.Cancelled, JoinedOn = new DateTime(2026, 10, 24), StatusChangedOn = new DateTime(2026, 11, 1), IsLateCancel = true },
                new MatchParticipant { Id = 8, MatchId = 6, UserId = SeedUserIds.User007, Status = ParticipantStatus.Joined, JoinedOn = new DateTime(2026, 10, 25), StatusChangedOn = new DateTime(2026, 10, 25), IsLateCancel = false },
                new MatchParticipant { Id = 9, MatchId = 7, UserId = SeedUserIds.User008, Status = ParticipantStatus.NoShow, JoinedOn = new DateTime(2026, 10, 26), StatusChangedOn = new DateTime(2026, 11, 11), IsLateCancel = false },
                new MatchParticipant { Id = 10, MatchId = 9, UserId = SeedUserIds.User010, Status = ParticipantStatus.Attended, JoinedOn = new DateTime(2026, 10, 27), StatusChangedOn = new DateTime(2026, 11, 15), IsLateCancel = false });
        }
    }
}

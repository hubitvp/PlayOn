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
                new MatchParticipant {
                    Id = 1,
                    MatchId = 1,
                    UserId = SeedUserIds.User001,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 9, 29, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 2,
                    MatchId = 1,
                    UserId = SeedUserIds.User002,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 0, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 3,
                    MatchId = 1,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 6, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 4,
                    MatchId = 1,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 12, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 5,
                    MatchId = 1,
                    UserId = SeedUserIds.User005,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 18, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 6,
                    MatchId = 1,
                    UserId = SeedUserIds.User006,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 0, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 7,
                    MatchId = 1,
                    UserId = SeedUserIds.User007,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 6, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 8,
                    MatchId = 1,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 12, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 9,
                    MatchId = 1,
                    UserId = SeedUserIds.User010,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 18, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 10,
                    MatchId = 1,
                    UserId = SeedUserIds.User008,
                    Status = ParticipantStatus.Cancelled,
                    JoinedOn = new DateTime(2026, 10, 3, 12, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 16, 50, 0),
                    IsLateCancel = true
                },
                new MatchParticipant {
                    Id = 11,
                    MatchId = 2,
                    UserId = SeedUserIds.User002,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 9, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 14, 20, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 12,
                    MatchId = 2,
                    UserId = SeedUserIds.User005,
                    Status = ParticipantStatus.NoShow,
                    JoinedOn = new DateTime(2026, 10, 5, 1, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 14, 20, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 13,
                    MatchId = 2,
                    UserId = SeedUserIds.User001,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 14, 20, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 14,
                    MatchId = 2,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 13, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 14, 20, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 15,
                    MatchId = 3,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 3, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 17, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 16,
                    MatchId = 3,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 7, 16, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 17, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 17,
                    MatchId = 3,
                    UserId = SeedUserIds.User010,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 7, 22, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 17, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 18,
                    MatchId = 3,
                    UserId = SeedUserIds.User001,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 8, 4, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 17, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 19,
                    MatchId = 4,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 20,
                    MatchId = 4,
                    UserId = SeedUserIds.User008,
                    Status = ParticipantStatus.NoShow,
                    JoinedOn = new DateTime(2026, 10, 8, 15, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 21,
                    MatchId = 4,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 8, 21, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 22,
                    MatchId = 4,
                    UserId = SeedUserIds.User005,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 9, 3, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 23,
                    MatchId = 4,
                    UserId = SeedUserIds.User006,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 9, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 24,
                    MatchId = 4,
                    UserId = SeedUserIds.User007,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 9, 15, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 25,
                    MatchId = 4,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 9, 21, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 26,
                    MatchId = 4,
                    UserId = SeedUserIds.User010,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 10, 3, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 27,
                    MatchId = 5,
                    UserId = SeedUserIds.User005,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 28,
                    MatchId = 5,
                    UserId = SeedUserIds.User001,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 29,
                    MatchId = 5,
                    UserId = SeedUserIds.User002,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 30,
                    MatchId = 5,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 31,
                    MatchId = 5,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Waitlisted,
                    JoinedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 32,
                    MatchId = 6,
                    UserId = SeedUserIds.User006,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 33,
                    MatchId = 6,
                    UserId = SeedUserIds.User005,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 34,
                    MatchId = 6,
                    UserId = SeedUserIds.User007,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 35,
                    MatchId = 7,
                    UserId = SeedUserIds.User007,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 36,
                    MatchId = 7,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 37,
                    MatchId = 7,
                    UserId = SeedUserIds.User010,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 38,
                    MatchId = 8,
                    UserId = SeedUserIds.User008,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 39,
                    MatchId = 8,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 40,
                    MatchId = 8,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 41,
                    MatchId = 8,
                    UserId = SeedUserIds.User005,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 42,
                    MatchId = 8,
                    UserId = SeedUserIds.User006,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 43,
                    MatchId = 9,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 11, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 44,
                    MatchId = 9,
                    UserId = SeedUserIds.User007,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 15, 20, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 45,
                    MatchId = 9,
                    UserId = SeedUserIds.User010,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 16, 2, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 46,
                    MatchId = 9,
                    UserId = SeedUserIds.User001,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 16, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 47,
                    MatchId = 9,
                    UserId = SeedUserIds.User002,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 16, 14, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 48,
                    MatchId = 9,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 16, 20, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 49,
                    MatchId = 9,
                    UserId = SeedUserIds.User008,
                    Status = ParticipantStatus.Cancelled,
                    JoinedOn = new DateTime(2026, 10, 15, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 12, 50, 0),
                    IsLateCancel = true
                },
                new MatchParticipant {
                    Id = 50,
                    MatchId = 10,
                    UserId = SeedUserIds.User010,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 51,
                    MatchId = 10,
                    UserId = SeedUserIds.User007,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 52,
                    MatchId = 10,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 53,
                    MatchId = 11,
                    UserId = SeedUserIds.User001,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 54,
                    MatchId = 11,
                    UserId = SeedUserIds.User002,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 28, 16, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 28, 16, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 55,
                    MatchId = 11,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 28, 22, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 28, 22, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 56,
                    MatchId = 11,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 29, 4, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 29, 4, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 57,
                    MatchId = 12,
                    UserId = SeedUserIds.User002,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 58,
                    MatchId = 12,
                    UserId = SeedUserIds.User005,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 59,
                    MatchId = 12,
                    UserId = SeedUserIds.User006,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 60,
                    MatchId = 12,
                    UserId = SeedUserIds.User007,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 61,
                    MatchId = 12,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 62,
                    MatchId = 12,
                    UserId = SeedUserIds.User010,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 11, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 11, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 63,
                    MatchId = 12,
                    UserId = SeedUserIds.User001,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 12, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 64,
                    MatchId = 12,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 13, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 13, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 65,
                    MatchId = 12,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Waitlisted,
                    JoinedOn = new DateTime(2026, 10, 30, 14, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 14, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 66,
                    MatchId = 13,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 67,
                    MatchId = 13,
                    UserId = SeedUserIds.User009,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 68,
                    MatchId = 13,
                    UserId = SeedUserIds.User010,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 69,
                    MatchId = 13,
                    UserId = SeedUserIds.User001,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 70,
                    MatchId = 13,
                    UserId = SeedUserIds.User002,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 71,
                    MatchId = 14,
                    UserId = SeedUserIds.User005,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 72,
                    MatchId = 14,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 73,
                    MatchId = 14,
                    UserId = SeedUserIds.User004,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 74,
                    MatchId = 15,
                    UserId = SeedUserIds.User003,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant {
                    Id = 75,
                    MatchId = 15,
                    UserId = SeedUserIds.User007,
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                });
        }
    }
}

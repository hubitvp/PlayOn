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
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant001),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 9, 29, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant002),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User002),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 0, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant003),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 6, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant004),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 12, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant005),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 18, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant006),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User006),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 0, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant007),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 6, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant008),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 12, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant009),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 18, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 19, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant010),
                    MatchId = Guid.Parse(SeedMatchIds.Match001),
                    UserId = Guid.Parse(SeedUserIds.User008),
                    Status = ParticipantStatus.Cancelled,
                    JoinedOn = new DateTime(2026, 10, 3, 12, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 13, 16, 50, 0),
                    IsLateCancel = true
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant011),
                    MatchId = Guid.Parse(SeedMatchIds.Match002),
                    UserId = Guid.Parse(SeedUserIds.User002),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 9, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 14, 20, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant012),
                    MatchId = Guid.Parse(SeedMatchIds.Match002),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Status = ParticipantStatus.NoShow,
                    JoinedOn = new DateTime(2026, 10, 5, 1, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 14, 20, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant013),
                    MatchId = Guid.Parse(SeedMatchIds.Match002),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 14, 20, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant014),
                    MatchId = Guid.Parse(SeedMatchIds.Match002),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 5, 13, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 14, 20, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant015),
                    MatchId = Guid.Parse(SeedMatchIds.Match003),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 3, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 17, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant016),
                    MatchId = Guid.Parse(SeedMatchIds.Match003),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 7, 16, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 17, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant017),
                    MatchId = Guid.Parse(SeedMatchIds.Match003),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 7, 22, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 17, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant018),
                    MatchId = Guid.Parse(SeedMatchIds.Match003),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 8, 4, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 17, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant019),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 4, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant020),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    UserId = Guid.Parse(SeedUserIds.User008),
                    Status = ParticipantStatus.NoShow,
                    JoinedOn = new DateTime(2026, 10, 8, 15, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant021),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 8, 21, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant022),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 9, 3, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant023),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    UserId = Guid.Parse(SeedUserIds.User006),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 9, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant024),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 9, 15, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant025),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 9, 21, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant026),
                    MatchId = Guid.Parse(SeedMatchIds.Match004),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 10, 3, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 18, 10, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant027),
                    MatchId = Guid.Parse(SeedMatchIds.Match005),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant028),
                    MatchId = Guid.Parse(SeedMatchIds.Match005),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant029),
                    MatchId = Guid.Parse(SeedMatchIds.Match005),
                    UserId = Guid.Parse(SeedUserIds.User002),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant030),
                    MatchId = Guid.Parse(SeedMatchIds.Match005),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant031),
                    MatchId = Guid.Parse(SeedMatchIds.Match005),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Waitlisted,
                    JoinedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant032),
                    MatchId = Guid.Parse(SeedMatchIds.Match006),
                    UserId = Guid.Parse(SeedUserIds.User006),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant033),
                    MatchId = Guid.Parse(SeedMatchIds.Match006),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant034),
                    MatchId = Guid.Parse(SeedMatchIds.Match006),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant035),
                    MatchId = Guid.Parse(SeedMatchIds.Match007),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant036),
                    MatchId = Guid.Parse(SeedMatchIds.Match007),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant037),
                    MatchId = Guid.Parse(SeedMatchIds.Match007),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant038),
                    MatchId = Guid.Parse(SeedMatchIds.Match008),
                    UserId = Guid.Parse(SeedUserIds.User008),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant039),
                    MatchId = Guid.Parse(SeedMatchIds.Match008),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant040),
                    MatchId = Guid.Parse(SeedMatchIds.Match008),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant041),
                    MatchId = Guid.Parse(SeedMatchIds.Match008),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant042),
                    MatchId = Guid.Parse(SeedMatchIds.Match008),
                    UserId = Guid.Parse(SeedUserIds.User006),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant043),
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 11, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant044),
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 15, 20, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant045),
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 16, 2, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant046),
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 16, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant047),
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    UserId = Guid.Parse(SeedUserIds.User002),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 16, 14, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant048),
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Attended,
                    JoinedOn = new DateTime(2026, 10, 16, 20, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 15, 30, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant049),
                    MatchId = Guid.Parse(SeedMatchIds.Match009),
                    UserId = Guid.Parse(SeedUserIds.User008),
                    Status = ParticipantStatus.Cancelled,
                    JoinedOn = new DateTime(2026, 10, 15, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 25, 12, 50, 0),
                    IsLateCancel = true
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant050),
                    MatchId = Guid.Parse(SeedMatchIds.Match010),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant051),
                    MatchId = Guid.Parse(SeedMatchIds.Match010),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant052),
                    MatchId = Guid.Parse(SeedMatchIds.Match010),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant053),
                    MatchId = Guid.Parse(SeedMatchIds.Match011),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant054),
                    MatchId = Guid.Parse(SeedMatchIds.Match011),
                    UserId = Guid.Parse(SeedUserIds.User002),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 28, 16, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 28, 16, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant055),
                    MatchId = Guid.Parse(SeedMatchIds.Match011),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 28, 22, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 28, 22, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant056),
                    MatchId = Guid.Parse(SeedMatchIds.Match011),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 29, 4, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 29, 4, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant057),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User002),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant058),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant059),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User006),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant060),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant061),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant062),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 11, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 11, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant063),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 12, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 12, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant064),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 13, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 13, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant065),
                    MatchId = Guid.Parse(SeedMatchIds.Match012),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Waitlisted,
                    JoinedOn = new DateTime(2026, 10, 30, 14, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 14, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant066),
                    MatchId = Guid.Parse(SeedMatchIds.Match013),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant067),
                    MatchId = Guid.Parse(SeedMatchIds.Match013),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant068),
                    MatchId = Guid.Parse(SeedMatchIds.Match013),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant069),
                    MatchId = Guid.Parse(SeedMatchIds.Match013),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant070),
                    MatchId = Guid.Parse(SeedMatchIds.Match013),
                    UserId = Guid.Parse(SeedUserIds.User002),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 10, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant071),
                    MatchId = Guid.Parse(SeedMatchIds.Match014),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant072),
                    MatchId = Guid.Parse(SeedMatchIds.Match014),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant073),
                    MatchId = Guid.Parse(SeedMatchIds.Match014),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 8, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant074),
                    MatchId = Guid.Parse(SeedMatchIds.Match015),
                    UserId = Guid.Parse(SeedUserIds.User003),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 20, 9, 0, 0),
                    IsLateCancel = false
                },
                new MatchParticipant
                {
                    Id = Guid.Parse(SeedMatchParticipantIds.Participant075),
                    MatchId = Guid.Parse(SeedMatchIds.Match015),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Status = ParticipantStatus.Joined,
                    JoinedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    StatusChangedOn = new DateTime(2026, 10, 30, 7, 0, 0),
                    IsLateCancel = false
                }
            );
        }
    }
}

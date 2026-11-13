using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlayOn.Common.Enums;
using PlayOn.Data.Configurations.SeedData;
using PlayOn.Data.Models;

namespace PlayOn.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(x => x.CreatedOn)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification001),
                    UserId = Guid.Parse(SeedUserIds.User010),
                    Type = NotificationType.MatchCancelled,
                    Message = "Baseball Practice Game was cancelled: Venue maintenance.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match010}",
                    IsRead = false,
                    CreatedOn = new DateTime(2026, 10, 30, 9, 0, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification002),
                    UserId = Guid.Parse(SeedUserIds.User007),
                    Type = NotificationType.MatchCancelled,
                    Message = "Baseball Practice Game was cancelled: Venue maintenance.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match010}",
                    IsRead = false,
                    CreatedOn = new DateTime(2026, 10, 30, 9, 0, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification003),
                    UserId = Guid.Parse(SeedUserIds.User009),
                    Type = NotificationType.MatchCancelled,
                    Message = "Baseball Practice Game was cancelled: Venue maintenance.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match010}",
                    IsRead = false,
                    CreatedOn = new DateTime(2026, 10, 30, 9, 0, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification004),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Type = NotificationType.MatchConfirmed,
                    Message = "Padel After Work has enough players and will go ahead.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match005}",
                    IsRead = true,
                    CreatedOn = new DateTime(2026, 10, 30, 10, 0, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification005),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Type = NotificationType.MatchConfirmed,
                    Message = "Padel After Work has enough players and will go ahead.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match005}",
                    IsRead = false,
                    CreatedOn = new DateTime(2026, 10, 30, 10, 0, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification006),
                    UserId = Guid.Parse(SeedUserIds.User004),
                    Type = NotificationType.Promoted,
                    Message = "A spot opened in Padel After Work. You moved off the waitlist.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match005}",
                    IsRead = false,
                    CreatedOn = new DateTime(2026, 10, 30, 18, 30, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification007),
                    UserId = Guid.Parse(SeedUserIds.User008),
                    Type = NotificationType.StrikeAdded,
                    Message = "You left Hockey Training Game less than 2 hours before the start. A strike was added.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match009}",
                    IsRead = true,
                    CreatedOn = new DateTime(2026, 10, 25, 12, 50, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification008),
                    UserId = Guid.Parse(SeedUserIds.User005),
                    Type = NotificationType.StrikeAdded,
                    Message = "You were marked as a no-show for Downtown Basketball. A strike was added.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match002}",
                    IsRead = false,
                    CreatedOn = new DateTime(2026, 10, 14, 20, 40, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification009),
                    UserId = Guid.Parse(SeedUserIds.User002),
                    Type = NotificationType.RatingReceived,
                    Message = "Alex Morgan rated you 5/5 after Tuesday Evening Football.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match001}",
                    IsRead = true,
                    CreatedOn = new DateTime(2026, 10, 14, 1, 30, 0)
                },
                new Notification
                {
                    Id = Guid.Parse(SeedNotificationIds.Notification010),
                    UserId = Guid.Parse(SeedUserIds.User001),
                    Type = NotificationType.RatingReceived,
                    Message = "Jordan Lee rated you 3/5 after Downtown Basketball.",
                    Link = $"/Matches/Details/{SeedMatchIds.Match002}",
                    IsRead = false,
                    CreatedOn = new DateTime(2026, 10, 15, 2, 30, 0)
                });
        }
    }
}

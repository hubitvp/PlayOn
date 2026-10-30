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
                new Notification { Id = 1, UserId = SeedUserIds.User001, Type = NotificationType.MatchConfirmed, Message = "Your football match has been confirmed.", Link = "/Matches/Details/1", IsRead = false, CreatedOn = new DateTime(2026, 10, 20) },
                new Notification { Id = 2, UserId = SeedUserIds.User002, Type = NotificationType.Promoted, Message = "You have been promoted from the waitlist.", Link = "/Matches/Details/4", IsRead = true, CreatedOn = new DateTime(2026, 10, 21) },
                new Notification { Id = 3, UserId = SeedUserIds.User003, Type = NotificationType.RatingReceived, Message = "You received a new player rating.", Link = "/Ratings", IsRead = false, CreatedOn = new DateTime(2026, 10, 22) },
                new Notification { Id = 4, UserId = SeedUserIds.User004, Type = NotificationType.MatchCancelled, Message = "A match you joined was cancelled.", Link = "/Matches/Details/10", IsRead = true, CreatedOn = new DateTime(2026, 10, 23) },
                new Notification { Id = 5, UserId = SeedUserIds.User005, Type = NotificationType.MatchConfirmed, Message = "Your padel match is confirmed.", Link = "/Matches/Details/5", IsRead = false, CreatedOn = new DateTime(2026, 10, 24) },
                new Notification { Id = 6, UserId = SeedUserIds.User006, Type = NotificationType.StrikeAdded, Message = "A late cancellation strike was added to your account.", Link = "/Account/Strikes", IsRead = true, CreatedOn = new DateTime(2026, 10, 25) },
                new Notification { Id = 7, UserId = SeedUserIds.User007, Type = NotificationType.RatingReceived, Message = "Your basketball rating is ready to view.", Link = "/Ratings", IsRead = false, CreatedOn = new DateTime(2026, 10, 26) },
                new Notification { Id = 8, UserId = SeedUserIds.User008, Type = NotificationType.MatchConfirmed, Message = "Your rugby match has enough players.", Link = "/Matches/Details/8", IsRead = true, CreatedOn = new DateTime(2026, 10, 27) },
                new Notification { Id = 9, UserId = SeedUserIds.User009, Type = NotificationType.Promoted, Message = "You have joined the hockey match.", Link = "/Matches/Details/9", IsRead = false, CreatedOn = new DateTime(2026, 10, 28) },
                new Notification { Id = 10, UserId = SeedUserIds.User010, Type = NotificationType.MatchCancelled, Message = "The baseball practice game was cancelled.", Link = "/Matches/Details/10", IsRead = false, CreatedOn = new DateTime(2026, 10, 29) });
        }
    }
}

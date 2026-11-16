namespace PlayOn.Web.ViewModels.Notifications
{
    public class NotificationListViewModel //: PageViewModel
    {
        public required IReadOnlyList<NotificationViewModel> Notifications { get; init; }

        public required int UnreadCount { get; init; }
    }

    public class NotificationViewModel
    {
        public required Guid Id { get; init; }
        public required string Message { get; init; }
        public Guid? MatchId { get; init; }
        public required bool IsRead { get; init; }
        public required string CreatedOn { get; init; }
        public required string IconCss { get; init; }
    }
}

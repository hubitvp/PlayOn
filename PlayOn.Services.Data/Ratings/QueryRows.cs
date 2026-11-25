using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Ratings
{
    public sealed class RatingInfoRow
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public MatchStatus Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}

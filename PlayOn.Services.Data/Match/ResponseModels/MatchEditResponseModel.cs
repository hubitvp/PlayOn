using PlayOn.Common.Enums;

namespace PlayOn.Services.Data.Match.ResponseModels
{


    public class MatchEditDisplayDataResponseModel
    {
        public required string SportName { get; init; }
        public required SportIcon SportIcon { get; init; }
        public required string CourtName { get; init; }
        public required int CourtCapacity { get; init; }
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public required DateOnly Date { get; init; }
        public required TimeOnly StartsAt { get; init; }
        public required TimeOnly ClosesAt { get; init; }

        public required int JoinedCount { get; init; }
    }



    public class MatchEditResponseModel
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string? Description { get; init; }
        public required int DurationMinutes { get; init; }
        public required int MinPlayers { get; init; }
        public required int MaxPlayers { get; init; }
        public required SkillLevel MinSkill { get; init; }
        public required SkillLevel MaxSkill { get; init; }
        public required decimal PricePerPlayer { get; init; }
        public required byte[] RowVersion { get; init; }


        public required MatchEditDisplayDataResponseModel EditDisplayDataResponseModel { get; set; }

    }
}

using PlayOn.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Web.ViewModels.Home
{
    public class HomeMatchCardViewModel
    {
        public required int Id { get; init; }
        public string Slug { get; set; } = null!;
        public required string Title { get; init; }
        public required string SportName { get; init; }
        public string SportIcon { get; set; } = null!;
        public required string VenueName { get; init; }
        public required string City { get; init; }
        public string StartsAt { get; set; } = null!;
        public required int DurationMinutes { get; init; }
        public required int Joined { get; init; }
        public required int MaxPlayers { get; init; }
        public required int Waitlisted { get; init; }
        public required SkillLevel? MinSkill { get; init; }
        public string PricePerPlayer { get; set; } = null!;
        public required MatchStatus Status { get; init; }
    }
}

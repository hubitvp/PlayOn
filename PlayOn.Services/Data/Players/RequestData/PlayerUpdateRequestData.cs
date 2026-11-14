using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.ApplicationUserConstants;

namespace PlayOn.Services.Data.Players.RequestData
{
    public class PlayerUpdateRequestData
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(DisplayNameMaxLength, MinimumLength = DisplayNameMinLength)]
        public string DisplayName { get; set; } = null!;

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = null!;

        public List<PlayerSkillRequestData> Skills { get; set; } = [];
    }

    public class PlayerSkillRequestData
    {
        public Guid SportId { get; set; }

        public SkillLevel? SkillLevel { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.ApplicationUserConstants;

namespace PlayOn.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(DisplayNameMaxLength)]
        public string DisplayName { get; set; } = null!;

        [Required]
        [MaxLength(CityMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }

        public int? ReliabilityScore { get; set; }

        [Required]
        public int MatchesPlayed { get; set; }
    }
}

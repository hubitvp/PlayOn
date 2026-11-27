using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.ApplicationUserConstants;

namespace PlayOn.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {

        [Required]
        [PersonalData]
        public override Guid Id { get; set; } = Guid.NewGuid();


        [Required]
        [PersonalData]
        [StringLength(FullNameMaxLength)]
        [Comment("Full name of the member")]
        public string FullName { get; set; } = null!;

        [PersonalData]
        [StringLength(AddressMaxLength)]
        [Comment("Optional home address of the member")]
        public string? Address { get; set; }

        [Required]
        [PersonalData]
        [Comment("Date the member joined")]
        public DateOnly JoinDate { get; set; }

        [Required]
        [PersonalData]
        [MaxLength(DisplayNameMaxLength)]
        public string DisplayName { get; set; } = null!;

        [Required]
        [PersonalData]
        [MaxLength(CityMaxLength)]
        public string City { get; set; } = null!;

        [Required]
        [PersonalData]
        public bool IsActive { get; set; }

        [PersonalData]
        public int? ReliabilityScore { get; set; }

        [Required]
        [PersonalData]
        public int MatchesPlayed { get; set; }
    }
}

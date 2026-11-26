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
        [StringLength(FullNameMaxLength)]
        [Comment("Full name of the member")]
        public string FullName { get; set; } = null!;

        [StringLength(AddressMaxLength)]
        [Comment("Optional home address of the member")]
        public string? Address { get; set; }

        [Required]
        [Comment("Date the member joined")]
        public DateOnly JoinDate { get; set; }

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

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.EntityConstants.ApplicationUserConstants;

namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents a registered member of the PlayOn platform.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        [Required]
        [PersonalData]
        [Comment("Unique identifier of the user")]
        public override Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Full name of the member.
        /// </summary>
        [Required]
        [PersonalData]
        [StringLength(FullNameMaxLength)]
        [Comment("Full name of the member")]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Optional home address of the member.
        /// </summary>
        [PersonalData]
        [StringLength(AddressMaxLength)]
        [Comment("Optional home address of the member")]
        public string? Address { get; set; }

        /// <summary>
        /// Date the member joined the platform.
        /// </summary>
        [Required]
        [PersonalData]
        [Comment("Date the member joined")]
        public DateOnly JoinDate { get; set; }

        /// <summary>
        /// Public display name of the member.
        /// </summary>
        [Required]
        [PersonalData]
        [MaxLength(DisplayNameMaxLength)]
        [Comment("Public display name of the member")]
        public string DisplayName { get; set; } = null!;

        /// <summary>
        /// City in which the member lives or plays.
        /// </summary>
        [Required]
        [PersonalData]
        [MaxLength(CityMaxLength)]
        [Comment("City in which the member lives or plays")]
        public string City { get; set; } = null!;

        /// <summary>
        /// Indicates whether the member account is active.
        /// </summary>
        [Required]
        [PersonalData]
        [Comment("Indicates whether the member account is active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Optional reliability score of the member, based on attendance and late cancellations.
        /// </summary>
        [PersonalData]
        [Comment("Optional reliability score of the member")]
        public int? ReliabilityScore { get; set; }

        /// <summary>
        /// Total number of matches the member has played.
        /// </summary>
        [Required]
        [PersonalData]
        [Comment("Total number of matches the member has played")]
        public int MatchesPlayed { get; set; }
    }
}

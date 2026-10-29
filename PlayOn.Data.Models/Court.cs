using PlayOn.Common.EntityConstants;
using PlayOn.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PlayOn.Data.Models
{
    public class Court
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(CourtConstants.NameMaxLenght)]
        public string Name { get; set; } = null!;


        [Required]
        public Surface Surface { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public decimal HourlyPrice { get; set; }

        [Required]
        public int VenueId { get; set; }

        [ForeignKey(nameof(VenueId))]
        public virtual Venue Venue { get; set; } = null!;


        [Required]
        public int SportId { get; set; }

        [ForeignKey(nameof(SportId))]
        public virtual Sport Sport { get; set; } = null!;

        public virtual ICollection<Match> Matches { get; set; }
            = new List<Match>();
    }
}

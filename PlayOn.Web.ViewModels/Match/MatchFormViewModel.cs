using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PlayOn.Web.ViewModels.Match
{

    public class MatchFormViewModel : MatchInputModelBase
    {
        [Range(1, int.MaxValue, ErrorMessage = "Select a sport.")]
        public int SportId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a court.")]
        public int CourtId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a date and time slot.")]
        public int VenueId { get; set; }


        public IReadOnlyList<SelectListItem> Sports { get; set; } = [];
        public IReadOnlyList<CourtOptionViewModel> Courts { get; set; } = [];
        public IReadOnlyList<VenueSlotOptionViewModel> Slots { get; set; } = [];
    }
}

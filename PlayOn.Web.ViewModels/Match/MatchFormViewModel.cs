using Microsoft.AspNetCore.Mvc.Rendering;

namespace PlayOn.Web.ViewModels.Match
{

    public class MatchFormViewModel : MatchInputModelBase
    {
        public Guid SportId { get; set; }

        public Guid CourtId { get; set; }

        public Guid VenueId { get; set; }


        public IEnumerable<SelectListItem> Sports { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<MatchCourtOptionViewModel> Courts { get; set; } = Enumerable.Empty<MatchCourtOptionViewModel>();
        public IEnumerable<MatchSlotOptionViewModel> Slots { get; set; } = Enumerable.Empty<MatchSlotOptionViewModel>();
    }
}

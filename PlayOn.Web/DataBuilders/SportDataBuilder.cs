using Microsoft.AspNetCore.Mvc.Rendering;
using PlayOn.Services.Data.Sport.ResponseModels;

namespace PlayOn.Web.DataBuilders
{
    public class SportDataBuilder
    {

        public SelectListItem
            SportToSelectListItem(
                SportResponseModel source,
                int? selectedSportId)
        {
            var result = new SelectListItem
            {
                Text = source.Name,
                Value = source.Id.ToString(),
                Selected = source.Id == selectedSportId
            };

            return result;
        }
    }
}

using PlayOn.Services.Data;
using PlayOn.Web.ViewModels;
using PlayOn.Web.ViewModels.Error;

namespace PlayOn.Web.DataBuilders
{
    public abstract class DataBuilder
    {
        public virtual PageRequestData PageInputToPageRequestData(PageInputModel source)
        {
            var result = new PageRequestData
            {
                Page = source.PageNumber,
                PageSize = source.PageSize,
            };
            return result;
        }


        public virtual ErrorViewModel ServiceStatusToErrorViewModel(ServiceStatus status) => status switch
        {
            ServiceStatus.Forbidden => new ErrorViewModel
            {
                Code = StatusCodes.Status403Forbidden,
                Title = "Access Denied",
                Message = "You shall not pass! Well, at least not without the right permissions."
            },

            ServiceStatus.NotFound => new ErrorViewModel
            {
                Code = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Message = "We looked under the couch cushions and behind the database, but this resource is nowhere to be found."
            },

            ServiceStatus.Conflict => new ErrorViewModel
            {
                Code = StatusCodes.Status409Conflict,
                Title = "Conflict",
                Message = "Looks like someone beat you to it, or two pieces of data are refusing to get along. Refresh and try again."
            },

            ServiceStatus.TimeLocked => new ErrorViewModel
            {
                Code = StatusCodes.Status423Locked,
                Title = "Time Locked",
                Message = "Hold your horses—this resource is locked in a time capsule right now. Come back when the clock says it's ready."
            },

            ServiceStatus.Success => throw new InvalidOperationException("Cannot map a Success status to an error view model."),

            _ => new ErrorViewModel
            {
                Code = StatusCodes.Status500InternalServerError,
                Title = "Unexpected Error",
                Message = "Something went sideways in a way we didn't even have a category for."
            }
        };
    }
}

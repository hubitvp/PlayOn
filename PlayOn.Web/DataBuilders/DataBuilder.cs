using PlayOn.Services.Data;
using PlayOn.Web.ViewModels;

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
    }
}

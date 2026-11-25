using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.ViewModels
{
    public class PageModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var pageNumberValue = bindingContext.ValueProvider.GetValue("page");
            var page = new PageInputModel();

            if (pageNumberValue != ValueProviderResult.None &&
                int.TryParse(
                    pageNumberValue.FirstValue,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out var parsedPageNumberValue))
            {
                page.PageNumber = parsedPageNumberValue;
            }

            bindingContext.Result = ModelBindingResult.Success(page);
            return Task.CompletedTask;
        }
    }

    [ModelBinder(BinderType = typeof(PageModelBinder))]
    public class PageInputModel
    {
        private int pageNumber = FirstPage;

        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = Math.Max(FirstPage, value);
        }

        public int PageSize { get; } = DefaultPageSize;
    }
}
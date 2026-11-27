using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Web.ViewModels
{
    /// <summary>
    /// Model binder that creates a <see cref="PageInputModel"/> from the <c>page</c> request value.
    /// </summary>
    public class PageModelBinder : IModelBinder
    {
        /// <summary>
        /// Creates a <see cref="PageInputModel"/> from the <c>page</c> request value. A missing or invalid value leaves the default first page.
        /// </summary>
        /// <param name="bindingContext">The context of the model binding.</param>
        /// <returns>A completed task.</returns>
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

    /// <summary>
    /// Input model holding the requested page number and page size.
    /// </summary>
    [ModelBinder(BinderType = typeof(PageModelBinder))]
    public class PageInputModel
    {
        /// <summary>
        /// Backing field of <see cref="PageNumber"/>.
        /// </summary>
        private int pageNumber = FirstPage;

        /// <summary>
        /// Requested page number. Values below the first page are corrected to the first page.
        /// </summary>
        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = Math.Max(FirstPage, value);
        }

        /// <summary>
        /// Number of items per page; always the default page size.
        /// </summary>
        public int PageSize { get; } = DefaultPageSize;
    }
}
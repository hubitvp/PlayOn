using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Web.ViewModels.Error
{
    public class ErrorViewModel
    {

        public int Code { get; init; }
        public string Title { get; init; } = null!;
        public string Message { get; init; } = null!;

    }
}

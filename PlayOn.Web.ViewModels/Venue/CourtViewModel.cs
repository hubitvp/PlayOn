using PlayOn.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Web.ViewModels.Venue
{
    public class CourtViewModel
    {
        public required string Name { get; init; }
        public required string SportIconCss { get; init; }
        public required string Surface { get; init; }
        public required int Capacity { get; init; }
        public required string HourlyPrice { get; init; }
    }
}

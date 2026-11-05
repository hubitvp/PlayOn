using PlayOn.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Services.Repositories.Data.Output
{
    public class CourtDto
    {
        public required string Name { get; init; }
        public required SportIcon SportIcon { get; init; }
        public required Surface Surface { get; init; }
        public required int Capacity { get; init; }
        public required decimal HourlyPrice { get; init; }
    }
}

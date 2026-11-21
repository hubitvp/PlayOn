using System;
using System.Collections.Generic;
using System.Text;

namespace PlayOn.Services.Data.Home.ResponseModels
{
    public class HomeSportOptionsResponseModel
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; } = null!;
    }
}

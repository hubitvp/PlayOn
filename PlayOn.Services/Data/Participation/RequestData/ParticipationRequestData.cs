using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;
using static PlayOn.Common.ApplicationConstants.PaginationConstants;

namespace PlayOn.Services.Data.Participation.RequestData
{
    public class ParticipationRequestData
    {
        public Guid MatchId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }

    public class MyMatchesRequestData
    {
        [Required]
        public Guid UserId { get; set; }

        [EnumDataType(typeof(MyMatchesTab))]
        public MyMatchesTab Tab { get; set; } = MyMatchesTab.Upcoming;

        public int Page { get; set; } = FirstPage;

        public int PageSize { get; set; } = DefaultPageSize;
    }
}

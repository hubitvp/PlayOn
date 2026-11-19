using PlayOn.Common.Enums;
using System.ComponentModel.DataAnnotations;

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
    }
}

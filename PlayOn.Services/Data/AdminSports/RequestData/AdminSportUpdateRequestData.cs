using PlayOn.Common.Validation;

namespace PlayOn.Services.Data.AdminSports.RequestData
{
    public class AdminSportUpdateRequestData : AdminSportRequestDataBase
    {
        [NotEmptyGuid(ErrorMessage = "The sport is not valid.")]
        public Guid SportId { get; set; }
    }
}

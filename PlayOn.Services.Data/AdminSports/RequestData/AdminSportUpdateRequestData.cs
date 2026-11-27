using PlayOn.Common.Validation;

namespace PlayOn.Services.Data.AdminSports.RequestData
{
    /// <summary>
    /// Request data for updating an existing sport.
    /// </summary>
    public class AdminSportUpdateRequestData : AdminSportRequestDataBase
    {
        /// <summary>
        /// Identifier of the sport to update.
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "The sport is not valid.")]
        public Guid SportId { get; set; }
    }
}

namespace PlayOn.Services.Data.Home.ResponseModels
{
    /// <summary>
    /// A sport option offered by the home page filter.
    /// </summary>
    public class HomeSportOptionsResponseModel
    {
        /// <summary>
        /// Unique identifier of the sport.
        /// </summary>
        public required Guid Id { get; init; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public required string Name { get; init; }
    }
}
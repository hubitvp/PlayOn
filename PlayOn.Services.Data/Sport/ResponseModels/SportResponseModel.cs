namespace PlayOn.Services.Data.Sport.ResponseModels
{
    /// <summary>
    /// Response model describing a sport.
    /// </summary>
    public class SportResponseModel
    {
        /// <summary>
        /// Unique identifier of the sport.
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// Name of the sport.
        /// </summary>
        public string Name { get; init; } = null!;
    }
}

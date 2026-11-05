namespace PlayOn.Services.Repositories.Data.Input
{
    public class MatchQueryDto
    {
        public int? SportId { get; set; }

        public string? City { get; set; }

        public DateOnly? From { get; set; }
    }
}

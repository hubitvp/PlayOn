
namespace PlayOn.Services.Data.Home.RequestData
{

    public class HomeSearchRequestData
    {
        public Guid? SportId { get; set; }

        public string? City { get; set; }

        public DateOnly? From { get; set; }
    }
}

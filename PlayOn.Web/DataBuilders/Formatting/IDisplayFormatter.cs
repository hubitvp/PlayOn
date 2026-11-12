namespace PlayOn.Web.DataBuilders.Formatting
{

    public interface IDisplayFormatter
    {
        string DateTime(DateTime value);

        string ShortDateTime(DateTime value);

        string Date(DateOnly value);

        string ShortDate(DateOnly value);

        string Time(TimeOnly value);

        string Time(DateTime value);

        string Price(decimal value);
    }
}

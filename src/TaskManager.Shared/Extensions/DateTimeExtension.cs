namespace TaskManager.Shared.Extensions;

public static class DateTimeExtension
{
    public static string ToDefaultFormat(this DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yyyy hh:mm:ss tt");
    }
}
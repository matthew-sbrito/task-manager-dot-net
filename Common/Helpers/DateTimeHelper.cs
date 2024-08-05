namespace Common.Helpers;

public static class DateTimeHelper
{
    public static DateTime UtcNow()
    {
        return DateTimeOffset.Now.DateTime;
    }

    public static long Timestamp()
    {
        return new DateTimeOffset(UtcNow()).ToUnixTimeMilliseconds();
    }
}
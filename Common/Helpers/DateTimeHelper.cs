namespace Common.Helpers;

public static class DateTimeHelper
{
    public static DateTime UtcNow()
    {
        return DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
    }

    public static long Timestamp()
    {
        return new DateTimeOffset(UtcNow()).ToUnixTimeMilliseconds();
    }
}
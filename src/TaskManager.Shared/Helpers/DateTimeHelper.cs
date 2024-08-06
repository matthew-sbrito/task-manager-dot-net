namespace TaskManager.Shared.Helpers;

/// <summary>
/// Class helper to get DateTime based on app rules
/// </summary>
public static class DateTimeHelper
{
    public static DateTime UtcNow()
    {
        return DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    }

    public static long Timestamp()
    {
        return new DateTimeOffset(UtcNow()).ToUnixTimeMilliseconds();
    }
}
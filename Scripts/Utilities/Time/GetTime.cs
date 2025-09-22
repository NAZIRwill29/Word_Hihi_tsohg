using UnityEngine;
using System.Text;

public static class GetTime
{
    [System.ThreadStatic] private static StringBuilder sb; // Unique per thread

    public static string GetCurrentTime(string format = "full")
    {
        sb ??= new StringBuilder(20); // Initialize per thread

        // Use UtcNow for better performance (avoiding timezone calculations)
        System.DateTime currentTime = System.DateTime.UtcNow;

        int hour = currentTime.Hour;
        int minute = currentTime.Minute;
        int second = currentTime.Second;
        int millisecond = currentTime.Millisecond;

        sb.Length = 0; // Faster than Clear()

        return format.ToLower() switch
        {
            "hour" => sb.Append(hour.ToString("D2")).ToString(),
            "hour-minute" => sb.Append(hour.ToString("D2")).Append(':').Append(minute.ToString("D2")).ToString(),
            "full" => sb.Append(hour.ToString("D2")).Append(':').Append(minute.ToString("D2"))
                              .Append(':').Append(second.ToString("D2")).ToString(),
            "full-ms" => sb.Append(hour.ToString("D2")).Append(':').Append(minute.ToString("D2"))
                              .Append(':').Append(second.ToString("D2")).Append('.').Append(millisecond.ToString("D3")).ToString(),
            _ => throw new System.ArgumentException($"Invalid format: {format}. Allowed values are 'hour', 'hour-minute', 'full', or 'full-ms'.")
        };
    }

    public static string GetCurrentDate(string format = "full")
    {
        sb ??= new StringBuilder(20); // Ensure it's initialized per thread

        System.DateTime currentDate = System.DateTime.UtcNow;

        int year = currentDate.Year;
        int month = currentDate.Month;
        int day = currentDate.Day;

        sb.Length = 0; // Faster than Clear()

        return format.ToLower() switch
        {
            "year" => sb.Append(year).ToString(),
            "year-month" => sb.Append(year).Append('-').Append(month.ToString("D2")).ToString(),
            "full" => sb.Append(year).Append('-').Append(month.ToString("D2")).Append('-').Append(day.ToString("D2")).ToString(),
            _ => throw new System.ArgumentException($"Invalid format: {format}. Allowed values are 'year', 'year-month', or 'full'.")
        };
    }
}

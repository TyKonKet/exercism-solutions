public static class SwiftScheduling
{
    public static DateTime DeliveryDate(DateTime meetingStart, string description) => description switch
    {
        "NOW" => Parse_NOW(meetingStart),
        "ASAP" => Parse_ASAP(meetingStart),
        "EOW" => Parse_EOW(meetingStart),
        var desc when desc.StartsWith('Q') => Parse_Quarterly(meetingStart, int.Parse(desc[1..])),
        var desc when desc.EndsWith('M') => Parse_Monthly(meetingStart, int.Parse(desc[..^1])),
        _ => throw new ArgumentException("Invalid delivery description"),
    };

    private static DateTime Parse_NOW(DateTime meetingStart) => meetingStart.AddHours(2);

    private static DateTime Parse_ASAP(DateTime meetingStart)
    {
        if (meetingStart.Hour < 13)
        {
            // Meeting day at 5PM
            return new DateTime(meetingStart.Year, meetingStart.Month, meetingStart.Day, 17, 0, 0);
        }
        else
        {
            // Next day at 1PM
            return new DateTime(meetingStart.Year, meetingStart.Month, meetingStart.Day, 13, 0, 0).AddDays(1);
        }
    }

    private static DateTime Parse_EOW(DateTime meetingStart)
    {
        if (meetingStart.DayOfWeek >= DayOfWeek.Monday && meetingStart.DayOfWeek < DayOfWeek.Thursday)
        {
            // If meeting is before Thursday, return Friday at 5PM of the same week
            return new DateTime(meetingStart.Year, meetingStart.Month, meetingStart.Day, 17, 0, 0).AddDays(DayOfWeek.Friday - meetingStart.DayOfWeek);
        }
        else
        {
            // Otherwise, return Sunday at 8PM of the same week
            var daysUntilSunday = (DayOfWeek.Sunday - meetingStart.DayOfWeek + 7) % 7; // Calculate days until Sunday, +7 to handle negative values
            return new DateTime(meetingStart.Year, meetingStart.Month, meetingStart.Day, 20, 0, 0).AddDays(daysUntilSunday);
        }
    }

    private static DateTime Parse_Monthly(DateTime meetingStart, int targetMonth)
    {
        if (meetingStart.Month < targetMonth)
        {
            // At 8AM on the _first_ workday of this year's target month
            return FirstWorkdayOfMonth(meetingStart.Year, targetMonth, 8);
        }
        else
        {
            // At 8AM on the _first_ workday of next year's target month
            return FirstWorkdayOfMonth(meetingStart.Year + 1, targetMonth, 8);
        }
    }

    private static DateTime Parse_Quarterly(DateTime meetingStart, int targetQuarter)
    {
        if (meetingStart.GetQuarter() <= targetQuarter)
        {
            // At 8:00 on the _last_ workday of this year's target quarter
            return LastWorkdayOfMonth(meetingStart.Year, LastMonthOfQuarter(targetQuarter), 8);
        }
        else
        {
            // At 8:00 on the _last_ workday of next year's target quarter
            return LastWorkdayOfMonth(meetingStart.Year + 1, LastMonthOfQuarter(targetQuarter), 8);
        }
    }

    #region Helpers

    private static DateTime FirstWorkdayOfMonth(int year, int month, int targetHour)
    {
        // Start at the first day of the month
        var firstDay = new DateTime(year, month, 1, targetHour, 0, 0);
        while (firstDay.DayOfWeek == DayOfWeek.Saturday || firstDay.DayOfWeek == DayOfWeek.Sunday)
        {
            // while it's a weekend, move to the next day
            firstDay = firstDay.AddDays(1);
        }
        return firstDay;
    }

    private static DateTime LastWorkdayOfMonth(int year, int month, int targetHour)
    {
        // Start at the last day of the month
        var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month), targetHour, 0, 0);
        while (lastDay.DayOfWeek == DayOfWeek.Saturday || lastDay.DayOfWeek == DayOfWeek.Sunday)
        {
            // while it's a weekend, move to the previous day
            lastDay = lastDay.AddDays(-1);
        }
        return lastDay;
    }
    
    private static int LastMonthOfQuarter(int quarter) => quarter * 3;

    private static int GetQuarter(this DateTime date) => ((date.Month - 1) / 3) + 1;
    #endregion
}

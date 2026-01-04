class BirdCount
{
    private static readonly int[] _lastWeek = [0, 2, 5, 3, 7, 8, 4];
    
    private int[] birdsPerDay;

    public BirdCount(int[] birdsPerDay)
    {
        this.birdsPerDay = birdsPerDay;
    }

    public static int[] LastWeek() => _lastWeek;

    public int Today() => birdsPerDay[birdsPerDay.Length - 1];

    public void IncrementTodaysCount() => birdsPerDay[birdsPerDay.Length - 1]++;

    public bool HasDayWithoutBirds()
    {
        foreach (var dayCount in birdsPerDay)
        {
            if (dayCount == 0) {
                return true;
            }
        }
        return false;
    }

    public int CountForFirstDays(int numberOfDays)
    {
        var count = 0;
        for (var day = 0; day < numberOfDays; day++)
        {
            count += birdsPerDay[day];
        }
        return count;
    }

    public int BusyDays()
    {
        var busyDays = 0;
        foreach (var dayCount in birdsPerDay)
        {
            if (dayCount >= 5) {
                busyDays++;
            }
        }
        return busyDays;
    }
}

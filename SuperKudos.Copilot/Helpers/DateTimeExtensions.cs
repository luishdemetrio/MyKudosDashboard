namespace SuperKudos.Copilot.Helpers;

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }

    public static int ConvertPhraseToDate(string phrase)
    {
        int result = 0;

        DateTime? date = null;

        if (phrase.ToLower().Contains("week"))
        {
            date = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
        }
        else if (phrase.ToLower().Contains("today"))
        {
            date = DateTime.Today;
        }
        else if (phrase.ToLower().Contains("yesterday"))
        {
            date = DateTime.Today.AddDays(-1);
        }
        else if (phrase.ToLower().Contains("month"))
        {
            date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }
        else if (phrase.ToLower().Contains("quarter"))
        {
            date = new DateTime(DateTime.Today.Year, (DateTime.Today.Month - 1) / 3 * 3 + 1, 1);
        }
        else if (phrase.ToLower().Contains("semester"))
        {
            date = new DateTime(DateTime.Today.Year, (DateTime.Today.Month - 1) / 6 * 6 + 1, 1);
        }
        else if (phrase.ToLower().Contains("year"))
        {
            date = new DateTime(DateTime.Today.Year, 1, 1);
        }

        
        if (date.HasValue)
            result = DateTime.Today.Subtract(date.Value).Days;
        
        return result;
    }


}
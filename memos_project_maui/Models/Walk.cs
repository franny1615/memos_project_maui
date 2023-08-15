using SQLite;

namespace memos_project_maui.Models;

[Table("walks_table")]
public class Walk
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int DurationInSeconds { get; set; }
    public double DistanceInMiles { get; set; }

    public string DurationFormatted
    {
        get
        {
            TimeSpan span = TimeSpan.FromSeconds(DurationInSeconds);
            string spanStr = "";

            if (span.Hours > 0)
            {
                spanStr += $"{span.Hours}hr(s) ";
            }
            else if (span.Minutes > 0)
            {
                spanStr += $"{span.Minutes}min(s) ";
            }
            else if (span.Seconds > 0)
            {
                spanStr += $"{span.Seconds}s";
            }

            return spanStr;
        }
    }

    public string DistanceFormatted
    {
        get
        {
            return $"{Math.Round(DistanceInMiles, 2)}mi";
        }
    }
}

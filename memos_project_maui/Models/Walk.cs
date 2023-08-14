using SQLite;

namespace memos_project_maui.Models;

[Table("walks_table")]
public class Walk
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int DurationInSeconds { get; set; }
    public double DistanceInMiles { get; set; }
}

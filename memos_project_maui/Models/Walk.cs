using SQLite;

namespace memos_project_maui.Models;

[Table("walks_table")]
public class Walk
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int StartLongitude { get; set; }
    public int EndLongitude { get; set; }
    public int StartLatitude { get; set; }
    public int EndLatitude { get; set; }
    public int DurationInSeconds { get; set; }
    public int Distance { get; set; }
    public string WalkType { get; set; }
    public string AfterWalkImage { get; set; }
}

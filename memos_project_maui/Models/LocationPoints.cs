using SQLite;

namespace memos_project_maui.Models;

[Table("location_points_table")]
public class LocationPoints
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int WalkId { get; set; }
    public string LocationJsonString { get; set; }
}


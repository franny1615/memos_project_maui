namespace memos_project_maui;

public class Constants
{
    public const string DatabaseFilename = "Walks.db3";
    
    public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    public static readonly Color PrimaryColor = Color.FromArgb("#E3170A"); 
    public static readonly Color SecondaryColor = Color.FromArgb("#49111C");
    public static readonly Color ThirdColor = Color.FromArgb("#CE8147");

    public const string RefreshWalksKey = "kRefreshWalks";
}

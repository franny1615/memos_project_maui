using System.Text.Json;
using memos_project_maui.Models;
using SQLite;

namespace memos_project_maui.Database;

public interface IWalkingDatabase
{
    public Task<List<Walk>> GetWalksAsync();
    public Task<Walk> GetWalkByIdAsync(int id);
    public void SaveWalkAsync(Walk walk);
    public void DeleteWalkAsync(Walk walk);
    public void SaveLocationPointsForWalkAsync(
        Walk walk,
        List<Location> locations);
}

public class WalkingDatabase : IWalkingDatabase
{
    SQLiteAsyncConnection Database;

    public async Task Init()
    {
        if (Database is not null) 
        {
            return;
        }

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await Database.CreateTableAsync<Walk>();
        await Database.CreateTableAsync<LocationPoints>();
    }

    public async Task<List<Walk>> GetWalksAsync()
    {
        await Init();
        return await Database.Table<Walk>().ToListAsync();
    }

    public async Task<Walk> GetWalkByIdAsync(int id)
    {
        await Init();
        return await Database
            .Table<Walk>()
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
    }

    // returns last updated/inserted row
    public async void SaveWalkAsync(Walk walk)
    {
        await Init();
        if (walk.Id != 0)
            await Database.UpdateAsync(walk);
        else
            await Database.InsertAsync(walk);
    }

    public async void SaveLocationPointsForWalkAsync(
        Walk walk,
        List<Location> locations)
    {
        await Init();

        List<LocationPoints> points = new();
        locations.ForEach(async (loc) =>
        {
            try
            {
                LocationPoints point = new();
                point.LocationJsonString = JsonSerializer.Serialize(loc);
                points.Add(point);
            }
            catch { }
        });

        await Database.InsertAllAsync(points);
    }

    public async void DeleteWalkAsync(Walk walk)
    {
        await Init();

        await Database.DeleteAsync(walk);
        // TODO: find all location points for this walk and delete them as well. 
    }
}

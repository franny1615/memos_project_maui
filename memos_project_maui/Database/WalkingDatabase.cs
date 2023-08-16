using System.Text.Json;
using memos_project_maui.Models;
using SQLite;

namespace memos_project_maui.Database;

public interface IWalkingDatabase
{
    public Task<List<Walk>> GetWalksAsync();
    public Task<Walk> GetWalkByIdAsync(int id);
    public Task<int> SaveWalkAsync(Walk walk);
    public Task DeleteWalkAsync(Walk walk);
    public Task SaveLocationPointsForWalkAsync(
        Walk walk,
        List<Location> locations);
    public Task<List<LocationPoints>> GetLocationPointsForWalkIdAsync(int id);
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
    public async Task<int> SaveWalkAsync(Walk walk)
    {
        await Init();

        if (walk.Id != 0)
            await Database.UpdateAsync(walk);
        else
            await Database.InsertAsync(walk);

        return walk.Id;
    }

    public async Task SaveLocationPointsForWalkAsync(
        Walk walk,
        List<Location> locations)
    {
        await Init();

        List<LocationPoints> points = new();
        locations.ForEach((loc) =>
        {
            try
            {
                LocationPoints point = new();
                point.LocationJsonString = JsonSerializer.Serialize(loc);
                point.WalkId = walk.Id;
                points.Add(point);
            }
            catch { }
        });

        await Database.InsertAllAsync(points);
    }

    public async Task DeleteWalkAsync(Walk walk)
    {
        await Init();

        List<LocationPoints> relevantPoints = await GetLocationPointsForWalkIdAsync(walk.Id);
        relevantPoints.ForEach(async (point) =>
        {
            await Database.DeleteAsync(point);
        });
        await Database.DeleteAsync(walk);
    }

    public async Task<List<LocationPoints>> GetLocationPointsForWalkIdAsync(int id)
    {
        await Init();

        List<LocationPoints> points = await Database
            .Table<LocationPoints>()
            .Where((i) => i.WalkId == id)
            .ToListAsync();

        return points;
    }
}

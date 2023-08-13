using memos_project_maui.Models;
using SQLite;

namespace memos_project_maui.Database;

public interface IWalkingDatabase
{
    public Task<List<Walk>> GetWalksAsync();
    public Task<Walk> GetWalkById(int id);
    public Task<int> SaveWalk(Walk walk);
    public Task<int> DeleteItemAsync(Walk walk);
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
        var result = await Database.CreateTableAsync<Walk>();
    }

    public async Task<List<Walk>> GetWalksAsync()
    {
        await Init();
        return await Database.Table<Walk>().ToListAsync();
    }

    public async Task<Walk> GetWalkById(int id)
    {
        await Init();
        return await Database
            .Table<Walk>()
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> SaveWalk(Walk walk)
    {
        await Init();
        if (walk.Id != 0)
            return await Database.UpdateAsync(walk);
        else
            return await Database.InsertAsync(walk);
    }

    public async Task<int> DeleteItemAsync(Walk walk)
    {
        await Init();
        return await Database.DeleteAsync(walk);
    }
}

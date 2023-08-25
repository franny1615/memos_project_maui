using System.Text.Json;
using memos_project_maui.Database;
using memos_project_maui.Models;
using Timer = System.Timers.Timer;

namespace memos_project_maui.ViewModels;

public interface IMainViewModel
{
	public Task<Location> GetCurrentLocation();
    public void StartWalk(Action<int> secondPassed);
	public void EndWalk();
	public Task SaveWalk(
		List<Location> locationData,
		int durationInSeconds,
		double distanceInMiles);
	public Task<List<Location>> GetLocationsOfWalkAsync(Walk pastWalk);
	public Task DeleteWalk(Walk walk);
}

public class MainViewModel : IMainViewModel
{
	private readonly IWalkingDatabase _database;
	private Timer _walkTimer; 
	private int _totalDuration = 0;

	public MainViewModel(IWalkingDatabase database)
	{
		_database = database;
	}

	public void StartWalk(Action<int> secondPassed)
	{
		_totalDuration = 0;
		_walkTimer = new();
		_walkTimer.Interval = 1000;
        _walkTimer.Enabled = true;
        _walkTimer.Elapsed += (sender, args) =>
		{
            _totalDuration += 1;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                secondPassed(_totalDuration);
            });
        };
		_walkTimer.Start();
	}

	public void EndWalk()
	{
		_walkTimer.Stop();
		_walkTimer = null;
	}

	public async Task<Location> GetCurrentLocation()
	{
		try
		{
			GeolocationRequest request = new()
			{
				DesiredAccuracy = GeolocationAccuracy.Best
			};
			request.RequestFullAccuracy = true;
			Location location = await Geolocation.Default.GetLocationAsync(request);

			return location;
		}
		catch 
		{
			return null;
		}
	}

    public async Task SaveWalk(
		List<Location> locationData,
		int durationInSeconds,
		double distanceInMiles)
    {
		Walk newWalk = new Walk();
		newWalk.DurationInSeconds = durationInSeconds;
		newWalk.DistanceInMiles = distanceInMiles;

		int idOfInserted = await _database.SaveWalkAsync(newWalk);
		newWalk.Id = idOfInserted;

		await _database.SaveLocationPointsForWalkAsync(newWalk, locationData);
    }

	public async Task<List<Location>> GetLocationsOfWalkAsync(Walk pastWalk)
	{
		List<LocationPoints> points = await _database.GetLocationPointsForWalkIdAsync(pastWalk.Id);
		List<Location> locations = new();

		points.ForEach((point) =>
		{
			try
			{
				Location loc = JsonSerializer.Deserialize<Location>(point.LocationJsonString);
				locations.Add(loc);
			} catch { }
		});

		return locations;
	}

	public async Task DeleteWalk(Walk walk)
	{
		await _database.DeleteWalkAsync(walk);
	}
}


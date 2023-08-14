using memos_project_maui.Database;
using memos_project_maui.Models;

namespace memos_project_maui.ViewModels;

public interface IMainViewModel
{
	public Task<Location> GetCurrentLocation();
    public void StartTimer(Action<int> secondPassed);
	public void StopTimer();
	public void SaveWalk(
		List<Location> locationData,
		int durationInSeconds,
		double distanceInMiles);
}

public class MainViewModel : IMainViewModel
{
	private readonly IWalkingDatabase _database;
	private CancellationTokenSource _cancellationToken;
	private int _totalDuration = 0;

	public MainViewModel(IWalkingDatabase database)
	{
		_database = database;
	}

	public void StartTimer(Action<int> secondPassed)
	{
		_totalDuration = 0;
		_cancellationToken = new();

		Task.Run(() =>
		{
			TimerLogic(secondPassed);
		}, _cancellationToken.Token);
	}

	private async void TimerLogic(Action<int> secondPassed)
	{
        while (true)
        {
            if (_cancellationToken.Token.IsCancellationRequested)
                return;

            _totalDuration += 1;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                secondPassed(_totalDuration);
            });

            await Task.Delay(1000); // one second
        }
    }

	public void StopTimer()
	{
		_cancellationToken.Cancel();
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

    public void SaveWalk(
		List<Location> locationData,
		int durationInSeconds,
		double distanceInMiles)
    {
		Walk newWalk = new Walk();
		newWalk.DurationInSeconds = durationInSeconds;
		newWalk.DistanceInMiles = distanceInMiles;

		_database.SaveWalkAsync(newWalk);
		_database.SaveLocationPointsForWalkAsync(newWalk, locationData);
    }
}


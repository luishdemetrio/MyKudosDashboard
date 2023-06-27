using MyKudos.Communication.Helper.Interfaces;
using MyKudos.Communication.Helper.Services;

public class UserProfileTableCacheBackgroundTask : IHostedService, IDisposable
{
        private Timer _timer;

        private readonly string _graphServiceUrl;

        private IRestClientHelper _restClientHelper;

    private int _intervalInMinutes;

    public UserProfileTableCacheBackgroundTask(IConfiguration configuration)
    {
        _intervalInMinutes = int.Parse(configuration["ADSyncIntervalFromMinutes"]);

        _graphServiceUrl = configuration["graphServiceUrl"];

        _restClientHelper = new RestClientHelper(
                   new RestServiceToken(
                    clientId: configuration["ClientId"],
                    clientSecret: configuration["ClientSecret"],
                    tenantId: configuration["TenantId"],
                    exposedAPI: configuration["ExposedApi"]
                )
                );
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(_intervalInMinutes));
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        // Update the user profile table on database
        await _restClientHelper.GetApiData<bool>($"{_graphServiceUrl}AllUsers");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

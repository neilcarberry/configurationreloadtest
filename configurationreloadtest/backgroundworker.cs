
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;

namespace configurationreloadtest
{
    public class backgroundworker : BackgroundService
    {
        private readonly IOptionsMonitor<TestObject> testobjmonitor;
        private readonly Refresher refresher;
        private readonly IConfigurationRefresher configRefresher;
        private readonly ILogger<backgroundworker> logger;

        public backgroundworker(
            IOptionsMonitor<TestObject> testobjmonitor,
            IServiceProvider services,
            Refresher refresher,
            IConfigurationRefresher configRefresher, 
            ILogger<backgroundworker> logger)
        {
            this.testobjmonitor = testobjmonitor;
            this.refresher = refresher;
            this.configRefresher = configRefresher;
            this.logger = logger;
        }
        protected override  async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(800);
                await refresher.refreshconfig();
                logger.LogInformation(testobjmonitor.CurrentValue.Password);
            }
        }
    }
}

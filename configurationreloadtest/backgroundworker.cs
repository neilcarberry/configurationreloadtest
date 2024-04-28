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
            this.testobjmonitor.OnChange((testobj) => dothing(testobj));
            this.refresher = refresher;
            this.configRefresher = configRefresher;
            this.logger = logger;
        }
        public void dothing(TestObject x)
        {
            logger.LogInformation("first bit 1" + x.Password);
            logger.LogInformation("second bit 2" + testobjmonitor.CurrentValue.Password);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

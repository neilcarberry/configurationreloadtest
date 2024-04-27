using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace configurationreloadtest
{
    public class Refresher 
    {
        private readonly IConfigurationRefresher _refresher;

        public Refresher(IConfigurationRefresher refresher)
        {
            _refresher = refresher;
        }

        public async Task<bool> refreshconfig()
        {
            return await _refresher.TryRefreshAsync();
        }
    }
}

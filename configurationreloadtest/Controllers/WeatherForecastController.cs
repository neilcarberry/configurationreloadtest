using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;

namespace configurationreloadtest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly singleyclass _singleyclass;
        private readonly Refresher _refresher;
        private readonly IConfigurationRefresher _configRefresher;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _services;

        private readonly IOptionsMonitor<TestObject> _testobjmonitor;
        private readonly IOptionsSnapshot<TestObject> _testobjsnapshot;
        private readonly ScopyClass _testobjmonitorscoped;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            singleyclass singleyclass, ScopyClass testobjmonitorscoped, 
            IOptionsMonitor<TestObject> testobjmonitor, IOptionsSnapshot<TestObject> testobjsnapshot, 
            IServiceProvider services,
            Refresher refresher,
            IConfigurationRefresher configRefresher, IConfiguration configuration)
        {
            _logger = logger;
            _singleyclass = singleyclass;
            _testobjmonitorscoped = testobjmonitorscoped;
            _testobjmonitor = testobjmonitor;
            _testobjsnapshot = testobjsnapshot;
            _services = services;
            _refresher = refresher;
            _configRefresher = configRefresher;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("getobejcts")]
        public async Task<string> GetTestOb()
        {
            var scopedone = _testobjmonitorscoped.getpass();
            var thisone = _testobjmonitor.CurrentValue.Password;

            var ss = await _refresher.refreshconfig();
            var sdg = await _configRefresher.TryRefreshAsync();

            using var newscope = _services.CreateAsyncScope();
            var scopyclass = newscope.ServiceProvider.GetRequiredService<ScopyClass>();
            var singleyclass = newscope.ServiceProvider.GetRequiredService<singleyclass>();
            var scopysnapshot = newscope.ServiceProvider.GetRequiredService<IOptionsSnapshot<TestObject>>();
            var after = scopyclass.getpass();
            var singlyvalaue = singleyclass.getpass();

            var snapshot = _testobjsnapshot.Value.Password;
            var singly = _singleyclass.getpass();

            return $"scopedClass = {scopedone}\r\n" +
                $"newasyncScope= {after}\r\n" +
                $"ioptions = {thisone}\r\n" +
                $"snapshot = {snapshot}\r\n" +
                $"snapshotscoped = {scopysnapshot.Value.Password}\r\n" +
                $"singlyscoped = {singlyvalaue}\r\n" +
                $"singlyscoped = {singly}";
        }
    }
}

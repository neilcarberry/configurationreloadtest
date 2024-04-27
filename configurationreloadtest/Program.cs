using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace configurationreloadtest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAzureAppConfiguration();
            builder.Configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect("Endpoint=https://dynamicconfigurationtest.azconfig.io;Id=zQUi;Secret=6NZMtpadRYDLI/gJv7OrcJOjioXnLWPObXt0BTCA3Yo=")
                    .Select("TestApp:*", LabelFilter.Null)
                    .ConfigureRefresh(refreshOptions =>
                        refreshOptions
                            .Register("TestApp:Sentinel", refreshAll: true));

                options.ConfigureKeyVault(keyVaultOptions =>
                {
                    keyVaultOptions.SetCredential(new DefaultAzureCredential());
                    keyVaultOptions.SetSecretRefreshInterval(TimeSpan.FromSeconds(10));
                });

                builder.Services.AddSingleton(options.GetRefresher());
                builder.Services.AddSingleton<Refresher>();

            });

            builder.Services.AddHostedService<backgroundworker>();
            builder.Services.Configure<TestObject>(builder.Configuration.GetSection("TestApp:TestObject"));
            builder.Services.AddOptions<TestObject>().RegisterChangeTokenSource();
            builder.Services.AddScoped<ScopyClass>();
            builder.Services.AddScoped<singleyclass>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAzureAppConfiguration();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using Microsoft.Extensions.Options;

namespace configurationreloadtest
{
    public static class OptionsBuilderExtensions
    {
        public static OptionsBuilder<TOptions> RegisterChangeTokenSource<TOptions>(this OptionsBuilder<TOptions> optionsBuilder)
          where TOptions : class
        {
            optionsBuilder.Services.AddSingleton<IOptionsChangeTokenSource<TOptions>, ConfigurationChangeTokenSource<TOptions>>(
              sp =>
              {
                  return new ConfigurationChangeTokenSource<TOptions>(optionsBuilder.Name, sp.GetRequiredService<IConfiguration>());
              });
            return optionsBuilder;
        }
    }
}
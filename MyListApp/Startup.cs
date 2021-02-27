using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyListApp.Auth;
using MyListApp.Graph;

[assembly: FunctionsStartup(typeof(MyListApp.Startup))]
namespace MyListApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<AuthOptions>()
                           .Configure<IConfiguration>((settings, configuration) =>
                           {
                               configuration.GetSection("Auth").Bind(settings);
                           });

            builder.Services.AddOptions<ListOptions>()
                            .Configure<IConfiguration>((settings, configuration) =>
                            {
                                configuration.GetSection("List").Bind(settings);
                            });

            builder.Services.AddSingleton<IAuthService, AadAuthService>();
            builder.Services.AddSingleton<IListService, SharePointListService>();
            builder.Services.AddSingleton<IFileService, SharePointFileService>();
        }
    }
}


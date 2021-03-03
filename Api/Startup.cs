using Api.Service;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using System;

[assembly: FunctionsStartup(typeof(Api.Startup))]
namespace Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient<OTDBClient>(c =>
            {
                c.BaseAddress = new Uri("https://opentdb.com/", UriKind.Absolute);
            });
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Subscription.API.Installer
{
    public class HttpClientInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientHandler>();
            services.AddHttpContextAccessor();

            #region GO REST API Client
            string uri = configuration.GetValue<string>("GoRestApi:Uri") ?? "https://gorest.co.in/public/v2/";
            string accept = configuration.GetValue<string>("GoRestApi:Accept") ?? "application/json";

            services.AddHttpClient(name: "GoRestApi", client =>
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Add("accept", MediaTypeHeaderValue.Parse(accept).MediaType);
                client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", MediaTypeHeaderValue.Parse(accept).MediaType);
            });

            #endregion

            //anaother clients can be added here...
        }
    }
}

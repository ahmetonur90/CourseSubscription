using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Training.API.Installer
{
    public class HttpClientInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientHandler>();
            services.AddHttpContextAccessor();

            #region GO REST API Client
            string uri = configuration.GetValue<string>("GoRestApi:Uri");
            string accept = configuration.GetValue<string>("GoRestApi:Accept");

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

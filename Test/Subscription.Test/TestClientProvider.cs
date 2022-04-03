using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Subscription.API;
using System;
using System.Net.Http;

namespace Subscription.Test
{
    public class TestClientProvider
    {
        public HttpClient Client { get; set; }

        public TestClientProvider()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            Client = server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5002");
        }
    }
}

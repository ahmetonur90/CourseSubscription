using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Subscription.API.Installer
{
    public class DefaultInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton(configuration);
            services.AddCors();
            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
        }
    }
}

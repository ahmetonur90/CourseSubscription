using CourseSubscription.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Training.API.Installer
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

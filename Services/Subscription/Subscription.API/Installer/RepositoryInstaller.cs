using AutoMapper;
using CourseSubscription.Data.Abstract;
using CourseSubscription.Data.Repository.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Subscription.API.Installer
{
    public class RepositoryInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMapper, Mapper>();
            services.AddScoped<ICoursesRepository, EfCoursesRepository>();
            services.AddScoped<ITrainingsRepository, EfTrainingsRepository>();
            services.AddScoped<ISubscriptionsRepository, EfSubscriptionsRepository>();
        }
    }
}

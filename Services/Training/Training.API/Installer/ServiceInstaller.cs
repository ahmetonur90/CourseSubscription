using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Training.API.Installer
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMapper, Mapper>();
            services.AddScoped<ITrainingsService, TrainingsService>();
            services.AddScoped<ICoursesServices, CoursesServices>();
        }
    }
}

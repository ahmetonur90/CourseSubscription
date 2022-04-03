using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Training.API.Installer
{
    public class ApiVersioningInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(options =>
            {
                // In requests without specifying a version, the default version is accepted.
                options.AssumeDefaultVersionWhenUnspecified = true;

                // default version definitions
                options.DefaultApiVersion = ApiVersion.Default;

                //After the request, the version numbers in our application are transmitted in the Response Header.
                options.ReportApiVersions = true;

            });

            services.AddVersionedApiExplorer(
              options =>
              {
                  // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                  // note: the specified format code will format the version as "'v'major[.minor][-status]"
                  options.GroupNameFormat = "'v'VVV";
                  // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                  // can also be used to control the format of the API version in route templates
                  options.SubstituteApiVersionInUrl = true;
              });
        }
    }

}

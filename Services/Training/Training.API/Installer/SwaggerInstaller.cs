using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Training.API.Extensions.Swagger;

namespace Training.API.Installer
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallerServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Training APIs",
                        Description = "API for showing swagger",
                        Version = "v1"
                    });

                #region Can be added a Bearer Authorization if needed
                //c.AddSecurityDefinition("Bearer",
                //    new OpenApiSecurityScheme()
                //    {
                //        Name = "Authorization",
                //        Type = SecuritySchemeType.ApiKey,
                //        Scheme = "Bearer",
                //        BearerFormat = "JWT",
                //        In = ParameterLocation.Header,
                //        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                //    });

                //c.AddSecurityRequirement(
                //    new OpenApiSecurityRequirement
                //{
                //    {
                //          new OpenApiSecurityScheme
                //            {
                //                Reference = new OpenApiReference
                //                {
                //                    Type = ReferenceType.SecurityScheme,
                //                    Id = "Bearer"
                //                }
                //            },
                //            new string[] {}

                //    }
                //}); 
                #endregion

                c.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var versions = methodInfo.DeclaringType
                    .GetCustomAttributes(inherit: false)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == version);
                });

                c.TagActionsBy(
                    api =>
                    {
                        if (api.GroupName != null)
                        {
                            return new[] { api.GroupName };
                        }

                        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                        if (controllerActionDescriptor != null)
                        {
                            return new[] { controllerActionDescriptor.ControllerName };
                        }

                        throw new InvalidOperationException("Unable to determine tag for endpoint.");
                    });

                c.OperationFilter<SwaggerParameterFilters>();
                c.DocumentFilter<SwaggerVersionMapping>();
                c.EnableAnnotations();

                c.IncludeXmlComments(XmlCommentsFilePath);
                c.CustomSchemaIds(SchemaIdStrategy);
            });

        }
        private static string SchemaIdStrategy(Type currentClass)
        {
            string returnedValue = currentClass.FullName;
            if (returnedValue.StartsWith("CourseSubscription.Core.DTOs."))
                returnedValue = returnedValue.Replace("CourseSubscription.Core.DTOs.", string.Empty);
            return returnedValue;
        }

        #region properties

        /// <summary>
        /// Retrieves the absolute file path of the XML documentation file.
        /// </summary>
        public static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        #endregion
    }

}

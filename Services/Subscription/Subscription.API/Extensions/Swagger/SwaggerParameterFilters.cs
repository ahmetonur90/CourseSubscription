using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using static Subscription.API.Extensions.Swagger.Config;

namespace Subscription.API.Extensions.Swagger
{
    internal class SwaggerParameterFilters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            try
            {
                var maps = context.MethodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToList();
                var version = maps[0].MajorVersion;
                if (Config.CurrentVersioningMethod == VersioningType.CustomHeader && !context.ApiDescription.RelativePath.Contains("{version}"))
                {
                    operation.Parameters.Add(new OpenApiParameter { Name = Config.CustomHeaderParam, In = ParameterLocation.Header, Required = false, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString(version.ToString()) } });
                }
                else if (Config.CurrentVersioningMethod == VersioningType.QueryString && !context.ApiDescription.RelativePath.Contains("{version}"))
                {
                    operation.Parameters.Add(new OpenApiParameter { Name = Config.QueryStringParam, In = ParameterLocation.Query, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString(version.ToString()) } });
                }
                else if (Config.CurrentVersioningMethod == VersioningType.AcceptHeader && !context.ApiDescription.RelativePath.Contains("{version}"))
                {
                    operation.Parameters.Add(new OpenApiParameter { Name = "Accept", In = ParameterLocation.Header, Required = false, Schema = new OpenApiSchema { Type = "String", Default = new OpenApiString($"application/json;{Config.AcceptHeaderParam}=" + version.ToString()) } });
                }

                var versionParameter = operation.Parameters.Single(p => p.Name == "version");

                if (versionParameter != null)
                {
                    operation.Parameters.Remove(versionParameter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}

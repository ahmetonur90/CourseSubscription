using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Subscription.API.Extensions.Swagger
{
    internal class SwaggerVersionMapping : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var op = new OpenApiPaths();
            foreach (var (emptyKey, value) in swaggerDoc.Paths)
            {
                var completeKey = emptyKey.Replace(oldValue: "v{version}", swaggerDoc.Info.Version);
                op.Add(completeKey, value);
            }
            swaggerDoc.Paths = op;
        }
    }
}

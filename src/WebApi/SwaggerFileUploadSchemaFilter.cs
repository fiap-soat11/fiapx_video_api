using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi;

public class SwaggerFileUploadSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(IFormFile) || context.Type == typeof(Microsoft.AspNetCore.Http.IFormFile))
        {
            schema.Type = "string";
            schema.Format = "binary";
            schema.Description = "File upload";
        }
    }
}


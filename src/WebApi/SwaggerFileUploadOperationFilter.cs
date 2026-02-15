using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebApi;

public class SwaggerFileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) || 
                       p.ParameterType == typeof(IFormFile[]) ||
                       (p.ParameterType != null && p.ParameterType.GetProperties()
                           .Any(prop => prop.PropertyType == typeof(IFormFile))))
            .ToList();

        if (!fileParams.Any()) return;

        if (operation.Parameters != null)
        {
            var paramsToRemove = new List<OpenApiParameter>();
            foreach (var param in operation.Parameters)
            {
                if (fileParams.Any(fp => fp.Name == param.Name))
                {
                    paramsToRemove.Add(param);
                }
            }
            foreach (var param in paramsToRemove)
            {
                operation.Parameters.Remove(param);
            }
        }

        var properties = new Dictionary<string, OpenApiSchema>();
        var required = new HashSet<string>();

        foreach (var param in fileParams)
        {
            if (param.ParameterType == typeof(IFormFile))
            {
                var paramName = param.Name ?? "file";
                properties[paramName] = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary",
                    Description = "File to upload"
                };
                required.Add(paramName);
            }
            else if (param.ParameterType != null)
            {
                var fileProps = param.ParameterType.GetProperties()
                    .Where(p => p.PropertyType == typeof(IFormFile));
                
                foreach (var prop in fileProps)
                {
                    properties[prop.Name] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary",
                        Description = "File to upload"
                    };
                    required.Add(prop.Name);
                }
            }
        }

        operation.RequestBody = new OpenApiRequestBody
        {
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = properties,
                        Required = required
                    }
                }
            }
        };
    }
}

using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using NJsonSchema;
using NSwag.SwaggerGeneration.Processors;
using NSwag.SwaggerGeneration.Processors.Contexts;

// ReSharper disable once CheckNamespace
namespace NSwag.AspNetCore
{
    public static class SwaggerDocumentSettingsExtensions
    {
        public static SwaggerDocumentSettings AddODataQueryOptions(this SwaggerDocumentSettings settings)
        {
            settings.OperationProcessors.Add(new QueryOptionsProcessor());

            return settings;
        }

        private class QueryOptionsProcessor : IOperationProcessor
        {
            public Task<bool> ProcessAsync(OperationProcessorContext context)
            {
                var enableQueryAttribute = context.MethodInfo.GetCustomAttribute<EnableQueryAttribute>();
                if (enableQueryAttribute != null)
                {
                    context.OperationDescription.Operation.Parameters.Add(new SwaggerParameter
                    {
                        Name = "$skip",
                        Kind = SwaggerParameterKind.Query,
                        IsRequired = false,
                        Position = 0,
                        Schema = new JsonSchema4 { Type = JsonObjectType.Integer },
                        Description = "Skip items"
                    });
                    context.OperationDescription.Operation.Parameters.Add(new SwaggerParameter
                    {
                        Name = "$top",
                        Kind = SwaggerParameterKind.Query,
                        IsRequired = false,
                        Position = 1,
                        Schema = new JsonSchema4 { Type = JsonObjectType.Integer },
                        Description = "Top items"
                    });
                    context.OperationDescription.Operation.Parameters.Add(new SwaggerParameter
                    {
                        Name = "$orderby",
                        Kind = SwaggerParameterKind.Query,
                        IsRequired = false,
                        Position = 2,
                        Schema = new JsonSchema4 { Type = JsonObjectType.String },
                        Description = "Order items"
                    });
                    context.OperationDescription.Operation.Parameters.Add(new SwaggerParameter
                    {
                        Name = "$select",
                        Kind = SwaggerParameterKind.Query,
                        IsRequired = false,
                        Position = 3,
                        Schema = new JsonSchema4 { Type = JsonObjectType.String },
                        Description = "Select items"
                    });
                    context.OperationDescription.Operation.Parameters.Add(new SwaggerParameter
                    {
                        Name = "$where",
                        Kind = SwaggerParameterKind.Query,
                        IsRequired = false,
                        Position = 4,
                        Schema = new JsonSchema4 { Type = JsonObjectType.String },
                        Description = "Filter items"
                    });
                }
                return Task.FromResult(true);
            }
        }
    }
}

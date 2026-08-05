using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TrajectoryAdvAIcer.Api.Filters;

/// <inheritdoc cref="ISchemaFilter"/>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <inheritdoc cref="ISchemaFilter.Apply(OpenApiSchema, SchemaFilterContext)"/>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Type = "string";
            schema.Enum.Clear();
            var enumNames = Enum.GetNames(context.Type);
            foreach (var name in enumNames)
            {
                schema.Enum.Add(new OpenApiString(name));
            }
        }
    }
}

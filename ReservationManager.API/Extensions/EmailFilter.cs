using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ReservationManager.API.Extensions;

public class EmailFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        if (!operation.Parameters.Any(x => x.Name.Equals("email", StringComparison.OrdinalIgnoreCase)))
        {
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "email",
                In = ParameterLocation.Query,
                Description = "Email Parameter",
                Required = true,
            });
        }
    }
}
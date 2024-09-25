using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Services;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;


namespace ReservationManager.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserTypeService, UserTypeService>();
            services.AddScoped<IResourceTypeService, ResourceTypeService>();
            services.AddScoped<IReservationTypeService, ReservationTypeService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IReservationService, ReservationService>();

            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();
            services.AddScoped<IResourceTypeRepository, ResourceTypeRepository>();
            services.AddScoped<IReservationTypeRepository, ReservationTypeRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            return services;
        }

        public static void ConfigureProblemDetails(ProblemDetailsOptions opt, IHostEnvironment webHostEnvironment)
        {
            opt.MapToStatusCode<EntityNotFoundException>(StatusCodes.Status404NotFound);
            opt.MapToStatusCode<TimeOnlyException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<InvalidCodeTypeException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<DeleteNotPermittedException>(StatusCodes.Status403Forbidden);

            //fallback
            opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            opt.IncludeExceptionDetails = (_, _) => !webHostEnvironment.IsProduction();
        }


    }
}

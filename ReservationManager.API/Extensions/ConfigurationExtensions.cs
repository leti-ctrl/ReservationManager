using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Mappers;
using ReservationManager.Core.Services;
using ReservationManager.Core.Validators;
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
            services.AddScoped<IClosingCalendarService, ClosingCalendarService>();

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
            services.AddScoped<IClosingCalendarRepository, ClosingCalendarRepository>();

            return services;
        }

        public static void ConfigureProblemDetails(ProblemDetailsOptions opt, IHostEnvironment webHostEnvironment)
        {
            opt.MapToStatusCode<EntityNotFoundException>(StatusCodes.Status404NotFound);
            opt.MapToStatusCode<TimeOnlyException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<InvalidCodeTypeException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<DeleteNotPermittedException>(StatusCodes.Status403Forbidden);
            opt.MapToStatusCode<TimetableExistsException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<CreateClosingCalendarException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<UpdateNotPermittedException>(StatusCodes.Status403Forbidden);

            //fallback
            opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            opt.IncludeExceptionDetails = (_, _) => !webHostEnvironment.IsProduction();
        }

        public static IServiceCollection ConfigureValidators(this IServiceCollection services)
        {
            services.AddScoped<IClosingCalendarValidator, ClosingTimetableValidator>();
            services.AddScoped<IResourceValidator, ResourceValidator>();
            services.AddScoped<IResourceFilterValidator, ResourceFilterValidator>();
            
            return services;
        }
        
        public static IServiceCollection ConfigureMappers(this IServiceCollection services)
        {
            services.AddScoped<IResourceReservedMapper, ResourceReservedMapper>();
            
            return services;
        }
    }
}

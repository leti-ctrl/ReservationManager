using ReservationManager.Core.Builders;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Services;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories;
using ReservationManager.Persistence.Repositories.Base;
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
            services.AddScoped<ITimetableTypeService, TimetableTypeService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IEstabilishmentTimetableService, EstabilishementTimetableService>();

            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();
            services.AddScoped<IResourceTypeRepository, ResourceTypeRepository>();
            services.AddScoped<IReservationTypeRepository, ReservationTypeRepository>();
            services.AddScoped<ITimetableTypeRepository, TimetableTypeRepository>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IEstabilishmentTimetableRepository, EstabilishmentTimetableRepository>();

            return services;
        }

        public static void ConfigureProblemDetails(ProblemDetailsOptions opt, IHostEnvironment webHostEnvironment)
        {
            opt.MapToStatusCode<EntityNotFoundException>(StatusCodes.Status404NotFound);
            opt.MapToStatusCode<TimeOnlyException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<InvalidCodeTypeException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<DeleteNotPermittedException>(StatusCodes.Status403Forbidden);
            opt.MapToStatusCode<TimetableExistsException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<CreateEstabilishmentTimetableException>(StatusCodes.Status400BadRequest);

            //fallback
            opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            opt.IncludeExceptionDetails = (_, _) => !webHostEnvironment.IsProduction();
        }

        public static IServiceCollection ConfigureBuilders(this IServiceCollection services)
        {
            services.AddScoped<IEstabilishmentTimetableBuilderStrategy, ClosedTimetableBuilderStrategy>();
            services.AddScoped<IEstabilishmentTimetableBuilderStrategy, TimeReductionTimetableBuilderStrategy>();
            services.AddScoped<IEstabilishmentTimetableBuilderStrategy, NominalTimetableBuilderStrategy>();

            services.AddScoped<IEstabilishmentTimetableBuilderStrategyHandler, EstabilishmentTimetableBuilderStrategyHandler>();

            return services;
        }

        public static IServiceCollection ConfigureValidators(this IServiceCollection services)
        {
            services.AddScoped<IEstabilishmentTimetableValidator, EstabilishmentTimetableValidator>();
            
            return services;
        }
    }
}

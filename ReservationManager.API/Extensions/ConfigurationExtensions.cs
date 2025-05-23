﻿using ReservationManager.Cache.Redis;
using ReservationManager.Cache.Repositories;
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
            services.AddSingleton<IRedisService, RedisService>();

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IResourceTypeService, ResourceTypeService>();
            services.AddScoped<IReservationTypeService, ReservationTypeService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IClosingCalendarService, ClosingCalendarService>();
            
            services.AddScoped<IResourceFilterService, ResourceFilterService>();
            services.AddScoped<IClosingCalendarFilterService, ClosingCalendarFilterService>();

            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRoleRepository, RoleCachedRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<IResourceTypeRepository, ResourceTypeCachedRepository>();
            services.AddScoped<ResourceTypeRepository>();
            services.AddScoped<IReservationTypeRepository, ReservationTypeCachedRepository>();
            services.AddScoped<ReservationTypeRepository>();

            services.AddScoped<IUserRepository, UserCachedRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<IResourceRepository, ResourceCachedRepository>();
            services.AddScoped<ResourceRepository>();
            services.AddScoped<IReservationRepository, ReservationCachedRepository>();
            services.AddScoped<ReservationRepository>();
            services.AddScoped<IClosingCalendarRepository, ClosingCalendarCachedRepository>();
            services.AddScoped<ClosingCalendarRepository>();

            return services;
        }

        public static void ConfigureProblemDetails(ProblemDetailsOptions opt, IHostEnvironment webHostEnvironment)
        {
            opt.MapToStatusCode<EntityNotFoundException>(StatusCodes.Status404NotFound);
            opt.MapToStatusCode<InvalidCodeTypeException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<DeleteNotPermittedException>(StatusCodes.Status409Conflict);
            opt.MapToStatusCode<CreateNotPermittedException>(StatusCodes.Status400BadRequest);
            opt.MapToStatusCode<OperationNotPermittedException>(StatusCodes.Status403Forbidden);
            opt.MapToStatusCode<BadHttpRequestException>(StatusCodes.Status403Forbidden);
            opt.MapToStatusCode<NonExistentUserException>(StatusCodes.Status403Forbidden);
            opt.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
            opt.MapToStatusCode<ReservationException>(StatusCodes.Status400BadRequest);

            //fallback
            opt.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            opt.IncludeExceptionDetails = (_, _) => !webHostEnvironment.IsProduction();
        }

        public static IServiceCollection ConfigureValidators(this IServiceCollection services)
        {
            services.AddScoped<IClosingCalendarFilterDtoValidator, ClosingCalendarFilterDtoValidator>();
            services.AddScoped<IClosingCalendarValidator, ClosingCalendarValidator>();
            services.AddScoped<IResourceValidator, ResourceValidator>();
            services.AddScoped<IResourceFilterDtoValidator, ResourceFilterDtoValidator>();
            services.AddScoped<IReservationTypeValidator, ReservationTypeValidator>();
            services.AddScoped<IUpsertReservationValidator, UpsertReservationValidator>();
            
            return services;
        }
        
        public static IServiceCollection ConfigureMappers(this IServiceCollection services)
        {
            services.AddScoped<IResourceReservedMapper, ResourceReservedMapper>();
            
            return services;
        }
    }
}

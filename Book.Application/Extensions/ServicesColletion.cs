using Book.Application.Interfaces;
using Book.Application.Services;
using Book.Core.Repository;
using Book.Core.Repository.Base;
using Book.Infrastructure.Data;
using Book.Infrastructure.Repository;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Book.Application.Extensions;

public static class ServiceCollection 
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
        return services;
    }
    
    public static IServiceCollection RegisterDatabaseContext(this IServiceCollection services ,IConfiguration configuration)
    {
        services.AddDbContext<BookDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
        return services;
    }
    
    public static IServiceCollection AddKeycloackAuth(this IServiceCollection services ,IConfiguration configuration)
    {
        var authenticationOptions = configuration
            .GetSection(KeycloakAuthenticationOptions.Section)
            .Get<KeycloakAuthenticationOptions>();

        if (authenticationOptions != null) services.AddKeycloakAuthentication(authenticationOptions);

        var authorizationOptions = configuration
            .GetSection(KeycloakProtectionClientOptions.Section)
            .Get<KeycloakProtectionClientOptions>();

        var adminClientOptions = configuration
            .GetSection(KeycloakAdminClientOptions.Section)
            .Get<KeycloakAdminClientOptions>();

        if (adminClientOptions != null) services.AddKeycloakAdminHttpClient(adminClientOptions);

        if (authorizationOptions != null)
            services
                .AddAuthorization(o => o.AddPolicy("IsAdmin", b =>
                {
                    b.RequireRealmRoles("admin");
                    b.RequireResourceRoles("r-admin");
                    b.RequireRole("r-admin");
                }))
                .AddKeycloakAuthorization(authorizationOptions);
        
        return services;
    }
}
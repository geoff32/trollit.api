using Microsoft.AspNetCore.Authorization;

namespace TrollIt.Api.Authorization.DependencyInjection;

public static class WebApiAuthorizationExtensions
{
    public static IServiceCollection AddWebApiAuthorization(this IServiceCollection services) =>
        services
            .AddAuthorization()
            .AddSingleton<IAuthorizationHandler, TrollResourceAuthorizationHandler>();
}
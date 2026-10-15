using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Security
{
    public static class IServiceCollectionExtensions
    {
        extension(IServiceCollection services)
        {
            public IServiceCollection AddCustomSecurity(Action<SecurityOptionsBuilder>? builderOptions = null)
            {
                SecurityOptionsBuilder builder = new SecurityOptionsBuilder();

                if(builderOptions is not null)
                {
                    builderOptions(builder);
                }

                services.AddSingleton<CustomSecurityMiddleware>();
                services.AddSingleton(builder);

                if(builder.UseCors)
                {
                    services.AddCors(o =>
                    {
                        o.AddPolicy(builder.PolicyName, b =>
                        {
                            if(builder.Origins.Any())
                            {
                                b.WithOrigins(builder.Origins.ToArray());
                            }

                            if(builder.Methods.Any())
                            {
                                b.WithMethods(builder.Methods.ToArray() + "OPTIONS");
                            }

                            if(builder.Headers.Any())
                            {
                                b.WithHeaders(builder.Headers.Select(h => h.HeaderName).ToArray());
                            }
                        });
                    });
                }

                return services;
            }
        }

        extension(WebApplication application)
        {
            public WebApplication UseCustomSecurity()
            {
                IServiceProvider serviceProvider = application.Services;

                SecurityOptionsBuilder? securityOptionsBuilder = serviceProvider.GetService<SecurityOptionsBuilder>();

                if(securityOptionsBuilder is not null)
                {
                    if(securityOptionsBuilder.UseCors)
                        application.UseCors(securityOptionsBuilder.PolicyName);

                    application.UseMiddleware<CustomSecurityMiddleware>();
                }

                return application;
            }
        }
    }
}

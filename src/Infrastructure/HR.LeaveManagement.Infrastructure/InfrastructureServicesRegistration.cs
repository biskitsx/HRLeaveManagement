﻿using HR.LeaveManagement.Application.Contracts.Email;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Models.Email;
using HR.LeaveManagement.Infrastructure.EmailServices;
using HR.LeaveManagement.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HR.LeaveManagement.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSetting>(options => configuration.GetSection("EmailSettings").Bind(options));
        services.AddTransient<IEmailSender, EmailSender>();

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        return services;
    }
}

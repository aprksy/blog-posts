using api.Domain.Interfaces;
using api.Application.Usecases;
using api.Application.Interfaces;
using api.Application.Validators;
using api.Infrastructure.Data;
using api.Infrastructure.Data.Repositories;
using api.Infrastructure.Services.EmailSender;
using api.Infrastructure.Services.EmailSender.LocalService;
using api.Infrastructure.Services.DocSynchronizer;
using api.Infrastructure.Services.DocSynchronizer.LocalService;
using Microsoft.EntityFrameworkCore;

public class ServiceConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // cors
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder => builder
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .Build());
        });

        // ioc
        services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "Test"));

        services.AddScoped<DataSeeder>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IGetClients, GetClients>();
        services.AddScoped<ISearchClients, SearchClients>();
        services.AddScoped<IUpdateClient, UpdateClient>();
        services.AddScoped<ICreateClient, CreateClient>();
        services.AddScoped<IValidateUpdateClientParam, ValidateUpdateClientParam>();
        services.AddScoped<IValidateCreateClientParam, ValidateCreateClientParam>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IDocSynchronizer, DocSynchronizer>();
    }
}
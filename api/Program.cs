using api.Domain.Interfaces;
using api.Application.Adapters;
using api.Application.Usecases;
using api.Infrastructure.Data;
using api.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using api.Domain.Entities;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
ServiceConfiguration.Configure(services);

var app = builder.Build();
EndpointConfiguration.Configure(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// seed data
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    dataSeeder.Seed();
}

// run app
app.Run();
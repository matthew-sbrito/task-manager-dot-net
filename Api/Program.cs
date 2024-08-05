using Application.Extensions;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using Api.Extensions;
using Api.Filters;
using Api.Middlewares;
using Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Application configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var apiVersion = new ApiVersion(1, 0);

if (string.IsNullOrWhiteSpace(connectionString))
    throw new ApplicationException("Invalid connection string ");

builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Manager API", Version = "v1.0" });
    swaggerGenOptions.OperationFilter<RequiredHeaderParameterFilter>();
});

// Add default services to the container.
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = apiVersion;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Execute migration only is not testing
if (!builder.Environment.IsEnvironment("Testing"))
    DatabaseConfiguration.ExecuteMigrations(connectionString);

// Add app services to the container.
builder.Services
    .AddAutoMapper(typeof(Program))
    .MapAutoMapper()
    .AddDatabase(connectionString)
    .AddValidators()
    .AddServices()
    .AddInitializers()
    .AddTransient<HandleExceptionMiddleware>()
    .AddEndpoints()
    .AddHttpContextAccessor();

var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(apiVersion)
    .ReportApiVersions()
    .Build();

var versionGroup = app
    .MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app
    .MapEndpoints(versionGroup)
    .UseMiddleware<HandleExceptionMiddleware>()
    .UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// await app.RunInitializers();

app.Run();
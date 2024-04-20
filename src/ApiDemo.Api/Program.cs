using System.Text.Json.Serialization;
using ApiDemo.Api.Extensions;
using ApiDemo.Api.Handlers;
using ApiDemo.Application.Extensions;
using ApiDemo.Infrastructure.Extensions;
using ApiDemo.Infrastructure.Settings;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(options =>
{
    options.AddServerHeader = false;
}).ConfigureAppConfiguration((_, configuration) =>
{
    configuration.AddEnvironmentVariables();
});

var services = builder.Services;

services.AddControllers(
        options => { options.SuppressAsyncSuffixInActionNames = false; })
    .ConfigureApiBehaviorOptions(
        options => { options.SuppressModelStateInvalidFilter = true; })
    .AddJsonOptions(
        options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

services.AddRouting(options => { options.LowercaseUrls = true; });

services.AddExceptionHandler<ValidationExceptionHandler>();
services.AddExceptionHandler<NotFoundExceptionHandler>();
services.AddExceptionHandler<GlobalExceptionHandler>();
services.AddProblemDetails();

builder.Services.AddValidatorsFromAssemblyContaining<ApiDemo.Infrastructure.IAssemblyReference>(ServiceLifetime.Singleton);

builder.Services.AddOptions<MongoDbSettings>()
    .Bind(builder.Configuration.GetSection(nameof(MongoDbSettings)))
    .ValidateFluently()
    .ValidateOnStart();

services.AddAutoMapper(x => x.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));

services.RegisterApplication();
services.RegisterInfrastructure();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
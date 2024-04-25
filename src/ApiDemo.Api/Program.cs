using System.Text;
using System.Text.Json.Serialization;
using ApiDemo.Api.Extensions;
using ApiDemo.Api.Handlers;
using ApiDemo.Application.Extensions;
using ApiDemo.Infrastructure.Extensions;
using ApiDemo.Infrastructure.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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

services.AddValidatorsFromAssemblyContaining<ApiDemo.Infrastructure.IAssemblyReference>(ServiceLifetime.Singleton);

services.AddOptions<MongoDbSettings>()
    .Bind(builder.Configuration.GetSection(nameof(MongoDbSettings)))
    .ValidateFluently()
    .ValidateOnStart();

services.AddOptions<AuthSettings>()
    .Bind(builder.Configuration.GetSection(nameof(AuthSettings)))
    .ValidateFluently()
    .ValidateOnStart();

services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var authSettingsOptions = services.GetRequiredService<IOptions<AuthSettings>>();
        var authSettings = authSettingsOptions.Value;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authSettings.Issuer,
            ValidAudience = authSettings.Audience,
            ClockSkew = TimeSpan.FromMinutes(5),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Secret))
        };
    });

services.AddAuthorization();

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

app.UseHsts();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
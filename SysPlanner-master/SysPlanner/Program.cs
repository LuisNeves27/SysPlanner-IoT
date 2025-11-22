using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SysPlanner.Infrastructure.Contexts;
using SysPlanner.Services;
using SysPlanner.Services.IA;
using SysPlanner.Services.Interfaces;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ---------------------------------------------------------
        // CONFIGURAÇÕES (mantive sua configuração)
        // ---------------------------------------------------------
        builder.Configuration.AddEnvironmentVariables();

        var oracleConnectionString = builder.Configuration.GetConnectionString("Oracle")
            ?? Environment.GetEnvironmentVariable("ORACLE_CONNECTION_STRING");

        builder.Services.AddDbContext<SysPlannerDbContext>(options =>
            options.UseOracle(oracleConnectionString));

        // Health checks (mantido)
        builder.Services.AddHealthChecks()
            .AddCheck("Database", () =>
            {
                try
                {
                    using var scope = builder.Services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<SysPlannerDbContext>();
                    db.Database.CanConnect();
                    return HealthCheckResult.Healthy("Conectado ao banco de dados com sucesso.");
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy("Falha ao conectar ao banco.", ex);
                }
            });

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy => policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
        });

        // Controllers + JSON
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Versionamento e Swagger
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Injeção de dependência — seus serviços existentes
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddScoped<ILembreteService, LembreteService>();

        // NOVOS serviços IoT / Location
        builder.Services.AddScoped<ILocalizacaoService, LocalizacaoService>();
        builder.Services.AddScoped<IIoTService, IoTService>();

        builder.Services.AddScoped<IAService>();

        // Problem details, logging, OpenTelemetry (mantido)
        builder.Services.AddProblemDetails();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Information);

        builder.Services.AddOpenTelemetry()
        .WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SysPlanner"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter();
        });

        var app = builder.Build();

        // ---------------------------------------------------------
        // PIPELINE
        // ---------------------------------------------------------
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Serve arquivos estáticos da wwwroot (dashboard)
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseCors("AllowAll");

        app.MapControllers();
        app.MapHealthChecks("/health");

        app.Run();
    }
}

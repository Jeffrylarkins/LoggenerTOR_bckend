using EmployeeManagementAPI;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// 1. Configure Serilog with custom RollingFileSink and file size-based rolling (1 MB) for info and error logs
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()  // Enrich logs with context (request-specific details)
    .Enrich.WithEnvironmentName()  // Add environment details like machine name
    .Enrich.WithThreadId()  // Add thread ID
    .Enrich.WithProperty("ApplicationName", "EmployeeManagementAPI")  // Custom property for app name
    .MinimumLevel.Information()       // Info logs to log-information-<Counter>.json (with correct file naming and rolling)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Information) // <-- Key: Filter for EXACTLY Information                                                                 // Info logs to log-information-<Counter>.json (Only info logs, excluding errors)
        .WriteTo.File(
            path: "logs/log-information-.json",  // Log information level logs to a file with date and counter
            rollingInterval: RollingInterval.Day,  // Daily rolling
            fileSizeLimitBytes: 1_000_000,  // 1MB file size limit for rolling
            rollOnFileSizeLimit: true,  // Enable rolling when file size exceeds limit
            retainedFileCountLimit: 10, // Keep only 10 rolling files
            //restrictedToMinimumLevel: LogEventLevel.Information, // Only log Information and higher (but no Error logs here)
            formatter: new Serilog.Formatting.Json.JsonFormatter()))  // Log to file in JSON format

    // Error logs to log-error-<Counter>.json (Only error logs, excluding info logs)
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Level == LogEventLevel.Error) // <-- Key: Filter for EXACTLY Information 
        .WriteTo.File(
            path: "logs/log-error-.json",  // Log error level logs to a separate file
            rollingInterval: RollingInterval.Day,  // Daily rolling
            fileSizeLimitBytes: 1_000_000,  // 1MB file size limit for rolling
            rollOnFileSizeLimit: true,  // Enable rolling when file size exceeds limit
            retainedFileCountLimit: 10,  // Keep only 10 rolling files
            //restrictedToMinimumLevel: LogEventLevel.Error, // Only log Error and higher (no Info logs here)
            formatter: new Serilog.Formatting.Json.JsonFormatter()))  // Log to file in JSON format

    // Log to console in JSON format (logs both Info and Error)
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())  // Log to console in JSON format
    .CreateLogger();


builder.Host.UseSerilog();  // Use Serilog for logging
var allowedOrigin = builder.Configuration["REACT_APP_URL"];
Console.WriteLine("CORS Allowed Origin: " + allowedOrigin);
if (!string.IsNullOrEmpty(allowedOrigin)) {
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp", policy =>
        {
            policy.WithOrigins(allowedOrigin)  // Allow React app to access backend
                  .AllowAnyHeader()                    // Allow any headers in the request
                  .AllowAnyMethod();                    // Allow any HTTP methods (GET, POST, etc.)
        });
    });
}

builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseMiddleware<RequestGuidMiddleware>();

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

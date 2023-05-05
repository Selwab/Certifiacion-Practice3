using Microsoft.OpenApi.Models;
using Serilog;
using UPB.CoreLogic.Managers;
using UPB.CoreLogic.Models;
using UPB.PracticeThree.Middlewares;

//create the logger and setup your sinks, filters and properties

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} (Application Running) {NewLine}{Exception}")
    .CreateBootstrapLogger();

//Servidor
var builder = WebApplication.CreateBuilder(args);

if(builder.Environment.IsEnvironment("DEV"))
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} (Application Running) {NewLine}{Exception}")
        //.WriteTo.File("logDEV.log", outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} (Application Running) {NewLine}{Exception}", rollingInterval: RollingInterval.Day)
        .CreateBootstrapLogger();
}

if(builder.Environment.IsEnvironment("QA"))
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} (Application Running) {NewLine}{Exception}")
        .CreateBootstrapLogger();
}

//after create the builder - UseSerilog
builder.Host.UseSerilog();

// Add services to the container.
//Singleton vs Transient vs Scoped

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//Configure application
var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

IConfiguration Configuration = configurationBuilder.Build();
string siteTitle = Configuration.GetSection("Title").Value;
string filePath = Configuration.GetSection("FilePath").Value;

builder.Services.AddTransient(_=> new PatientManager(filePath));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = siteTitle
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();

app.MapControllers();

//Run app
app.Run();

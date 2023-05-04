using Microsoft.OpenApi.Models;
using Serilog;
using UPB.CoreLogic.Managers;
using UPB.CoreLogic.Models;

//create the logger and setup your sinks, filters and properties
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} (Application Running) {NewLine}{Exception}")
    .CreateBootstrapLogger();

//Servidor
var builder = WebApplication.CreateBuilder(args);

//after create the builder - UseSerilog
builder.Host.UseSerilog();

// Add services to the container.
//Singleton vs Transient vs Scoped
builder.Services.AddSingleton<PatientManager>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configure application
var configurationBuilder = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

IConfiguration Configuration = configurationBuilder.Build();

string siteTitle = Configuration.GetSection("Title").Value;

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

//app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();

//Run app
app.Run();

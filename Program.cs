using Microsoft.OpenApi.Models;
using DotNetEnv;



// Load .env file
Env.Load();


var builder = WebApplication.CreateBuilder(args);

//read environment variable

var apiEndpoint = Environment.GetEnvironmentVariable("APP_API_ENDPOINT");
var APP_NAME = Environment.GetEnvironmentVariable("APP_NAME");
var APP_VERSION = Environment.GetEnvironmentVariable("APP_VERSION");

Console.WriteLine(apiEndpoint);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = APP_NAME, Version = APP_VERSION });
});

var app = builder.Build();

// Enable Swagger in Development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", APP_NAME + " " + APP_VERSION);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

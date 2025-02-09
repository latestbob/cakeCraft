using Microsoft.OpenApi.Models;
using DotNetEnv;
using CakeCraftApi.Models;
using CakeCraftApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Load .env file
Env.Load();


var builder = WebApplication.CreateBuilder(args);

//read environment variable

var apiEndpoint = Environment.GetEnvironmentVariable("APP_API_ENDPOINT");
var APP_NAME = Environment.GetEnvironmentVariable("APP_NAME");
var APP_VERSION = Environment.GetEnvironmentVariable("APP_VERSION");

var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
var dbDatabase = Environment.GetEnvironmentVariable("DB_DATABASE");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

// Construct connection string dynamically using values from .env
// var connectionString = $"Server={dbServer};Database={dbDatabase};User={dbUser};Password={dbPassword};";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

 var jwtSettings = builder.Configuration.GetSection("JwtSettings");

 builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
        };
    });


// builder.Services.AddDbContext<CakeCraftDbContext>(options =>
//     options.UseMySQL(connectionString));
builder.Services.AddDbContext<CakeCraftDbContext>(options =>
    options.UseMySql(connectionString,
    ServerVersion.AutoDetect(connectionString)));

Console.WriteLine(apiEndpoint);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = APP_NAME, Version = APP_VERSION });
});

builder.Services.AddAuthorization();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Tivoli.BLL;
using Tivoli.Dal;

// ReSharper disable SuggestVarOrType_SimpleTypes

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
// Add services to the container.
services.AddDbContext<DbContext, TivoliContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

services.AddAuthentication();
services.ConfigureIdentity();
services.ConfigureJWT(builder.Configuration);

services.ConfigureDependencies();

services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "Tivoli.CustomerApi", Version = "v1" });

    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

/// <summary>
///     Redeclaration of Program class to change access modifier to public for use in tests.
/// </summary>
public partial class Program
{
}
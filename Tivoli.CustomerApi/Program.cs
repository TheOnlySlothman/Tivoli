using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Tivoli.CustomerApi;
using Tivoli.CustomerApi.Services;
using Tivoli.Dal;
using Tivoli.Dal.Repo;
using Tivoli.Models.Entity;

// ReSharper disable SuggestVarOrType_SimpleTypes

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
// Add services to the container.
services.AddDbContext<DbContext, TivoliContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

services.AddAuthentication();
services.ConfigureIdentity();
services.ConfigureJWT(builder.Configuration);

services.AddTransient<UnitOfWork>();
services.AddScoped<AuthManager>();

services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo {Title = "Tivoli.CustomerApi", Version = "v1"});

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
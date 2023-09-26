using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tivoli.AdminApi;
using Tivoli.Dal;
using Tivoli.Dal.Repo;
using Tivoli.Models.Entity;

// ReSharper disable SuggestVarOrType_SimpleTypes

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<DbContext, TivoliContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddScoped<UserManager<Customer>>();
builder.Services.AddAuthentication();

builder.Services.ConfigureIdentity();
// builder.Services.AddIdentity<Customer, IdentityRole>();

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
///    Redeclaration of Program class to change access modifier to public for use in tests.
/// </summary>
// ReSharper disable once UnusedType.Global
public partial class Program
{
}
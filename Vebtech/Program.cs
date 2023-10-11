using DAL.Data;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;
using BLL.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Vebtech;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(opt => opt.AddDefaultPolicy(
                policy => policy.AllowAnyHeader().AllowAnyMethod()));
        builder.Services.AddControllers();

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IValidationEmailService, ValidationEmailService>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("ApplicationDbContext"),
                b => b.MigrationsAssembly("DAL")));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Vebtech API",
                Version = "v1"
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }
}
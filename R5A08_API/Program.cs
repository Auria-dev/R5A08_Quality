using AutoMapper;
using Microsoft.EntityFrameworkCore;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Mapping;
using R508_Revisions.Model.Repository.Implementation;
using R508_Revisions.Model.Repository.Interface;

namespace R508_Revisions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Dependency Injection
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<IProductTypeRepository, ProductTypeRepository>();

            // Database connection
            builder.Services.AddDbContext<ProduitsDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DbCoreConnectionString")));

            // CORS
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowBlazor", policy => {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // AutoMapper configuration
            builder.Services.AddAutoMapper(cfg => {
                cfg.ShouldMapMethod = _ => false;
            }, typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowBlazor");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

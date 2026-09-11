
using Microsoft.EntityFrameworkCore;
using R508_Revisions.Model.EntityFramework;
using R508_Revisions.Model.Repository;
using R508_Revisions.Model.Repository.Implementation;

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
            builder.Services.AddScoped<IProduitRepository, ProduitRepository>();
            builder.Services.AddDbContext<ProduitsDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbCoreConnectionString")));

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
        }
    }
}

using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CQRS.Events;
using UnicornTalentAgency.CQRS.Read;
using UnicornTalentAgency.CQRS.Routes;
using UnicornTalentAgency.CQRS.Write;

namespace UnicornTalentAgency.CQRS
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(cfg =>{
                cfg.RegisterServicesFromAssemblyContaining<Program>();
            });

            builder.Services.AddDbContextPool<WriteDbContext>(options =>
            {
                var dbName = builder.Configuration["dbName"] ?? "unicorn-talent-agency";
                options.UseInMemoryDatabase(dbName);
            }).AddDbContextPool<ReadDbContext>(options =>
            {
                var dbName = builder.Configuration["dbName"] ?? "unicorn-talent-agency";
                options.UseInMemoryDatabase(dbName);
            });

            builder.Services.AddTransient<ViewsRefresher>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapUnicornRoutes()
                .MapCastingRoleRoutes();

            app.Run();
        }
    }
}
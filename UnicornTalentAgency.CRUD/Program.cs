using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CRUD.Persistence;
using UnicornTalentAgency.CRUD.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<UTADbContext>(options =>
{
    var dbName = builder.Configuration["dbName"] ?? "unicorn-talent-agency";
    options.UseInMemoryDatabase(dbName);
});

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

public partial class Program { }
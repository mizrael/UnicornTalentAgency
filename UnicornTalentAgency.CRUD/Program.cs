using Microsoft.EntityFrameworkCore;
using UnicornTalentAgency.CRUD.Persistence;
using UnicornTalentAgency.CRUD.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<UTADbContext>(options =>
    options.UseInMemoryDatabase("unicorns")
);

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

await DbSeeder.SeedAsync(app);

app.Run();

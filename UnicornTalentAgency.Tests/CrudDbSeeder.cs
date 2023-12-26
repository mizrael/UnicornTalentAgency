using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UnicornTalentAgency.CRUD.Persistence;
using UnicornTalentAgency.CRUD.Persistence.Entities;

namespace UnicornTalentAgency.Tests;

internal static class CrudDbSeeder
{
    public static async ValueTask SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        config["dbName"] = Guid.NewGuid().ToString();

        var dbContext = scope.ServiceProvider.GetRequiredService<UTADbContext>();

        AddUnicorns(dbContext);
        AddRoles(dbContext);

        await dbContext.SaveChangesAsync();
    }

    private static void AddRoles(UTADbContext dbContext)
    {
        dbContext.AddRange(new[]
        {
            new CastingRole()
            {
                Name = "The Great and Powerful Trixie",
                Description = "Trixie is looking for a unicorn to assist her in her magic show.",
                Location = "Canterlot",
                Pay = 1000,
                When = DateTime.UtcNow.AddDays(7)
            },
            new CastingRole()
            {
                Name = "Enchanted Forest Explorer",
                Description = "Looking for a unicorn adventurer to explore the magical wonders of the enchanted forest.",
                Location = "Enchanted Forest",
                Pay = 1500,
                When = DateTime.UtcNow.AddDays(14)
            },
            new CastingRole()
            {
                Name = "Mystical Meadow Guardian",
                Description = "Seeking a unicorn with protective abilities to be the guardian of the mystical meadow.",
                Location = "Mystical Meadow",
                Pay = 1200,
                When = DateTime.UtcNow.AddDays(10)
            },
            new CastingRole()
            {
                Name = "Rainbow Rider",
                Description = "Unicorn needed for a high-flying adventure, riding rainbows across the sky.",
                Location = "Cloudsdale",
                Pay = 1800,
                When = DateTime.UtcNow.AddDays(21)
            },
            new CastingRole()
            {
                Name = "Starlight Spectacle",
                Description = "Searching for a unicorn to create a mesmerizing starlight display for a magical event.",
                Location = "Stellar City",
                Pay = 2000,
                When = DateTime.UtcNow.AddDays(28)
            }
        });
    }

    private static void AddUnicorns(UTADbContext dbContext)
    {
        dbContext.AddRange(new[]
        {
            new Unicorn()
            {
                Name = "Twilight Sparkle",
                MagicalAbilities = "Magic"
            },new Unicorn()
            {
                Name = "Rainbow Dash",
                MagicalAbilities = "Speed"
            },new Unicorn()
            {
                Name = "Rarity",
                MagicalAbilities = "Generosity"
            },new Unicorn()
            {
                Name = "Applejack",
                MagicalAbilities = "Honesty"
            }
        });
    }
}
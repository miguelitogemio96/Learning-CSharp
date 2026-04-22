using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
    // After installing ef, and EntityFramework.Design
    // Run Command: dotnet ef migrations add InitialCreate --output-dir Data/Migrations
    // dotnet ef database update, or check the DataExtensions to automate it.
}

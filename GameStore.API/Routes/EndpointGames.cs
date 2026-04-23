using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Routes;

public static class EndpointGames
{
    const string GetGameEndpointName = "GetGame";

    public static void MapGameEndpoints(this WebApplication app)
    {
        var gamesGroup = app.MapGroup("/games");

        // GET /games
        gamesGroup.MapGet("/", async (GameStoreContext dbContext) =>
        {
            var games = await dbContext.Games.Include(game => game.Genre).Select(game => new GameSummaryDto(
                game.Id,
                game.Name,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate
            )).AsNoTracking().ToListAsync();

            return Results.Ok(games);
        });

        // GET /games/{id}
        gamesGroup.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            Game? game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        })
        .WithName(GetGameEndpointName);

        // POST /games
        gamesGroup.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto createdGame = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = createdGame.Id }, createdGame);
        });

        // PUT /games/{id}
        gamesGroup.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var gameFound = await dbContext.Games.FindAsync(id);

            if (gameFound is null)
            {
                return Results.NotFound();
            }

            gameFound.Name = updatedGame.Name;
            gameFound.GenreId = updatedGame.GenreId;
            gameFound.Price = updatedGame.Price;
            gameFound.ReleaseDate = updatedGame.ReleaseDate;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games/{id}
        gamesGroup.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}

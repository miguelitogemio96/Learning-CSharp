using GameStore.API.DTOs;

namespace GameStore.API.Routes;

public static class EndpointGames
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameDto> games = [
        new (1, "Street Fighter II", "Fighting", 19.99M, new DateOnly(1992, 7, 15)),
        new (2, "Final Fantasy VII Rebirth", "RPG", 69.99M, new DateOnly(2024, 2, 29)),
        new (3, "Astro Bot", "Platformer", 59.99M, new DateOnly(2024, 9, 6)),
    ];

    public static void MapGameEndpoints(this WebApplication app)
    {

        var gamesGroup = app.MapGroup("/games");
        // GET /games
        gamesGroup.MapGet("/", () => games);

        // GET /games/{id}
        gamesGroup.MapGet("/{id}", (int id) =>
        {
            GameDto? foundGame = games.Find(game => game.Id == id);

            return foundGame is null ? Results.NotFound() : Results.Ok(foundGame);
        })
        .WithName(GetGameEndpointName);

        // POST /games
        gamesGroup.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
            games.Count + 1,
            newGame.Name,
            newGame.Genre,
            newGame.Price,
            newGame.ReleaseDate
            );

            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        // PUT /games/{id}
        gamesGroup.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            int gameIndex = games.FindIndex(game => game.Id == id);

            if (gameIndex == -1)
            {
                return Results.NotFound();
            }

            games[gameIndex] = new(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.NoContent();
        });

        // DELETE /games/{id}
        gamesGroup.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
    }
}

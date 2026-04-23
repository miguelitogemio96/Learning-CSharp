using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Routes;

public static class EndpointsGenres
{
    private const string GetGenreEndpointName = "GetGenreEndpointName";

    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        // GET /genres
        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
            var genres = await dbContext.Genres.Select(genre => new GenreDto(genre.Id, genre.Name)).AsNoTracking().ToListAsync();

            return Results.Ok(genres);
        });

        // GET /genres/id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var foundGenre = await dbContext.Genres.FindAsync(id);

            return foundGenre is null ? Results.NotFound() : Results.Ok(new GenreDto(foundGenre.Id, foundGenre.Name));
        }).WithName(GetGenreEndpointName);

        // POST /genres
        group.MapPost("/", async (CreateGenreDto createDto, GameStoreContext dbContext) =>
        {
            Genre newGenre = new()
            {
                Name = createDto.Name
            };

            dbContext.Add(newGenre);
            await dbContext.SaveChangesAsync();

            GenreDto createdGenre = new(newGenre.Id, newGenre.Name);

            return Results.CreatedAtRoute(GetGenreEndpointName, new { id = newGenre.Id }, createdGenre);
        });

        // PUT /genres/id
        group.MapPut("/{id}", async (int id, GenreDto updateGenre, GameStoreContext dbContext) =>
        {
            var foundGenre = await dbContext.Genres.FindAsync(id);

            if (foundGenre is null)
            {
                return Results.NotFound();
            }

            foundGenre.Name = updateGenre.Name;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /genres/id
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Genres.Where(genre => genre.Id == id).ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }

}

namespace GameStore.API.DTOs;

public record CreatedGameDto(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);

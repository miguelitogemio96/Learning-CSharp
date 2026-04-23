using System.ComponentModel.DataAnnotations;

namespace GameStore.API.DTOs;

public record CreateGameDto(
    [Required][StringLength(100)] string Name,
    [Required][Range(1, 1000)] int GenreId,
    [Required][Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);

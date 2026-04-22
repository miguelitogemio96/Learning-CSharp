using System.ComponentModel.DataAnnotations;

namespace GameStore.API.DTOs;

public record class UpdateGameDto(
    [Required][StringLength(100)] string Name,
    [Required][StringLength(20)] string Genre,
    [Required][Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);

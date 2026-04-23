using System.ComponentModel.DataAnnotations;

namespace GameStore.API.DTOs;

public record class CreateGenreDto(
    [Required][StringLength(100)] string Name
);

namespace GameStore.Api.Dtos;

public record class GameDetailsDto (
    int Id, 
    string Name,
    int GenreId,
    decimal Price,
    DateTime ReleaseDate);
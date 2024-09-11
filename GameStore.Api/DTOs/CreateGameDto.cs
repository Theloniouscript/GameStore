namespace GameStore.Api.Dtos;

public record class CreateGameDto ( 
    string Name,
    int GenreId,
    decimal Price,
    DateTime ReleaseDate);
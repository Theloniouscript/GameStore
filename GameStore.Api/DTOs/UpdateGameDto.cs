namespace GameStore.Api.Dtos;

public record class UpdateGameDto ( 
    string Name,
    int GenreId,
    decimal Price,
    DateTime ReleaseDate);
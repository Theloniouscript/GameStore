namespace GameStore.Api.Dtos;

public record class UpdateGameDto ( 
    string Name,
    string Genre,
    decimal Price,
    DateTime ReleaseDate);
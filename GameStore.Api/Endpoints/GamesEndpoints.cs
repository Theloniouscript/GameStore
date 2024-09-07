using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints {
    const string GetGameEndPointName = "GetGame";

    private static readonly List<GameDto> games = new List<GameDto>
    {
        new GameDto (
            1,
            "Street Fighter II",
            "Fighting",
            19.99M,
            new DateTime(1992, 7, 15, 0, 0, 0, DateTimeKind.Utc)),

        new GameDto (
            2, 
            "Final Fantasy XIV",
            "Roleplaying",
            59.99M,
            new DateTime(2010, 9, 30, 0, 0, 0, DateTimeKind.Utc)),

        new GameDto (
            3,
            "FIFA 23",
            "Sports",
            69.99M,
            new DateTime(2022, 9, 27, 0, 0, 0, DateTimeKind.Utc))
    };

    public static WebApplication MapGamesEndpoints (this WebApplication app) {
        // GET /games
        app.MapGet("games", () => games);

        // GET /games/1

        app.MapGet("games/{id}", (int id) => {
            GameDto? game = games.Find(game => game.Id == id);    
            return game is null ? Results.NotFound() : Results.Ok(game);
            })
            .WithName(GetGameEndPointName);

        // POST /games

        app.MapPost("games", (CreateGameDto newGame) => {

            if(string.IsNullOrEmpty(newGame.Name) || newGame.Name.Length > 50)
            {
                return Results.BadRequest("Name is required and should be < or = 50 characters");
            }

            if(string.IsNullOrEmpty(newGame.Genre) || newGame.Genre.Length > 20)
            {
                return Results.BadRequest("Genre is required and should be < or = 20 characters");
            }

            if(newGame.Price < 1 || newGame.Price > 100)
            {
                return Results.BadRequest("Price must be between 1 and 100");
            }

            GameDto game = new (
                games.Count +1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

                games.Add(game);

                return Results.CreatedAtRoute( GetGameEndPointName, new { id = game.Id}, game);

        });

        // PUT /games

        app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame) => {
            var index = games.FindIndex(game => game.Id == id);

            if(index == -1) {
                return Results.NotFound();
            }

            if(string.IsNullOrEmpty(updatedGame.Name) || updatedGame.Name.Length > 50)
            {
                return Results.BadRequest("Name is required and should be < or = 50 characters");
            }

            if(string.IsNullOrEmpty(updatedGame.Genre) || updatedGame.Genre.Length > 20)
            {
                return Results.BadRequest("Genre is required and should be < or = 20 characters");
            }

            if(updatedGame.Price < 1 || updatedGame.Price > 100)
            {
                return Results.BadRequest("Price must be between 1 and 100");
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate);

                return Results.NoContent();
        });

        // DELETE /games

        app.MapDelete("games/{id}", (int id) => {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return app;

    }
}
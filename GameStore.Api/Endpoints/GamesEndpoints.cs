using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints {
    const string GetGameEndPointName = "GetGame";

    private static readonly List<GameSummaryDto> games = new List<GameSummaryDto>
    {
        new GameSummaryDto (
            1,
            "Street Fighter II",
            "Fighting",
            19.99M,
            new DateTime(1992, 7, 15, 0, 0, 0, DateTimeKind.Utc)),

        new GameSummaryDto (
            2, 
            "Final Fantasy XIV",
            "Roleplaying",
            59.99M,
            new DateTime(2010, 9, 30, 0, 0, 0, DateTimeKind.Utc)),

        new GameSummaryDto (
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

        app.MapGet("games/{id}", (int id, GameStoreContext dbContext) => {
            Game? game = dbContext.Games.Find(id);    
            return game is null ? 
                Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
            })
            .WithName(GetGameEndPointName);

        // POST /games

        app.MapPost("games", (CreateGameDto newGame, GameStoreContext dbContext) => {

            if(string.IsNullOrEmpty(newGame.Name) || newGame.Name.Length > 50)
            {
                return Results.BadRequest("Name is required and should be < or = 50 characters");
            }

            if(newGame.Price < 1 || newGame.Price > 100)
            {
                return Results.BadRequest("Price must be between 1 and 100");
            }

            Game game = newGame.ToEntity();

            dbContext.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute( 
                GetGameEndPointName, 
                new { id = game.Id }, 
                game.ToGameDetailsDto());

        });

        // PUT /games

        app.MapPut("games/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {
            var existingGame = dbContext.Games.Find(id);

            if(existingGame is null) {
                return Results.NotFound();
            }

            if(string.IsNullOrEmpty(updatedGame.Name) || updatedGame.Name.Length > 50)
            {
                return Results.BadRequest("Name is required and should be < or = 50 characters");
            }

            if(updatedGame.Price < 1 || updatedGame.Price > 100)
            {
                return Results.BadRequest("Price must be between 1 and 100");
            }

            dbContext.Entry(existingGame)
                    .CurrentValues
                    .SetValues(updatedGame.ToEntity(id));
            dbContext.SaveChanges();

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
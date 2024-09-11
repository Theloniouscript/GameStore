using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints {
    const string GetGameEndPointName = "GetGame";
    public static WebApplication MapGamesEndpoints (this WebApplication app) {
        // GET /games
        app.MapGet("games", (GameStoreContext dbContext) => 
                dbContext.Games
                    .Include(game => game.Genre)
                    .Select(game => game.ToGameSummaryDto())
                    .AsNoTracking());

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

        app.MapDelete("games/{id}", (int id, GameStoreContext dbContext) => {

            var game = dbContext.Games.Find(id);

            if(game is null)
            {
                return Results.NotFound();
            }

            dbContext.Games.Remove(game);
            dbContext.SaveChanges();                             

            return Results.NoContent();
        });

        return app;

    }
}
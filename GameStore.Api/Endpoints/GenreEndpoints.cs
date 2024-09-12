using GameStore.Api.Data;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenreEndpoints
{
    public static WebApplication MapGenresEndpoints (this WebApplication app) 
    {
        app.MapGet("genres", async (GameStoreContext dbContext) =>
            await dbContext.Genres!
                .Select(genre => genre.ToDto())
                .AsNoTracking()
                .ToListAsync());

        return app;
    }
}
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext : DbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options) 
        : base (options)
        {
        }
    public DbSet<Game>? Games {get; set;} 
    public DbSet<Genre>? Genres {get; set;} 


}
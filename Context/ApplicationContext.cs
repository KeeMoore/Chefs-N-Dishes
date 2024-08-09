using Microsoft.EntityFrameworkCore;
using Chefs_N_Dishes.Models;

namespace Chefs_N_Dishes.Context;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions options) : base(options) { }
    public DbSet<Chef> Chefs { get; set; }
    public DbSet<Dish> Dishes { get; set; }

}

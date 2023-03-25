using Microsoft.EntityFrameworkCore;

namespace WeatherReport.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
           : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Variable> Variables { get; set; }

    }

}

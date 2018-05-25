using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Entities
{
    public sealed class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> dbContextOptions) : base(dbContextOptions)
        {
            //this call would create the db if it didn't exist
            Database.EnsureCreated();
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        //This is one of the ways to connect our context to the DB
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //}
    }
}

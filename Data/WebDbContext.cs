using LinkMaker.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkMaker.Data
{
    public class WebDbContext: DbContext
    {
        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options)
        {
        }
        public DbSet<Link> Links { get; set; }
    }
}
//this file acts as a bridge between the application and the database, allowing us to perform CRUD operations on the Link model. It inherits from DbContext, which is a part of Entity Framework Core, and it defines a DbSet<Link> property that represents the Links table in the database. The constructor takes DbContextOptions<WebDbContext> as a parameter, which allows us to configure the database connection when we set up the services in Program.cs.
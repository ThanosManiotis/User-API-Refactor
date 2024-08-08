using Microsoft.EntityFrameworkCore;
using Tests.User.Api.Models;

namespace Tests.User.Api
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}

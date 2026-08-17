using Microsoft.EntityFrameworkCore;

namespace Student_last_version.models
{
    public class AppDBContexts: DbContext
    {

        public AppDBContexts(DbContextOptions<AppDBContexts> options) : base(options) { }


        public DbSet<Student> Students { get; set; }







    }
}

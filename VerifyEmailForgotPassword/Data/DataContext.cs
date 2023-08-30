using Microsoft.EntityFrameworkCore;
using VerifyEmailForgotPassword.Controllers.Models;

namespace VerifyEmailForgotPassword.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=ALMALI\\OMERFDEV;Initial Catalog=USER-DB;User ID=sa;pwd=Omer34;");
        }
        public DbSet<User> Users =>Set<User>(); 
    }
}

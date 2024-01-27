using COMP2139_Lab1.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Lab1.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        }

        public DbSet<Project> Projects { get; set; }
    }
}

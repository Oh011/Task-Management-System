using Domain.Entities.IdentityModels;
using Domain.Entities.ProjectModels;
using Domain.Entities.TaskModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {




        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        }




        public DbSet<TaskItem> Tasks { get; set; }

        public DbSet<ProjectTask> projectTasks { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}

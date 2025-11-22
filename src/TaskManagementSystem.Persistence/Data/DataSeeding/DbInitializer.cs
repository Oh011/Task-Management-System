using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Data.DataSeeding
{
    public class DbInitializer : IDbInitializer
    {

        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;




        public DbInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            _context = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        public Task InitializeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task InitializeIdentityAsync()
        {




            if (!_roleManager.Roles.Any())
            {

                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));


                if (!_userManager.Users.Any())
                {


                    var AdminUser = new ApplicationUser()
                    {

                        FullName = "Admin",
                        UserName = "Admin",
                        Email = "Admin@Gmail.com",
                        PhoneNumber = "01145608910"


                    };


                    var user = new ApplicationUser()
                    {

                        FullName = "User",
                        UserName = "User",
                        Email = "User@Gmail.com",
                        PhoneNumber = "01145608910"

                    };

                    await _userManager.CreateAsync(AdminUser, "Admin@12");
                    await _userManager.CreateAsync(user, "User@123");


                    await _userManager.AddToRoleAsync(AdminUser, "Admin");
                    await _userManager.AddToRoleAsync(user, "User");

                }
            }
        }
    }
}

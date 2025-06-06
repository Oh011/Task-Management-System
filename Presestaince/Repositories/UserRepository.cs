using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
    {
        public async Task<IEnumerable<TResult>> GetAllWithProjectionSpecifications<TResult>(ProjectionSpecifications<ApplicationUser, TResult> specifications)
        {
            return await ApplyProjectionSpecifications(specifications).ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllWithSpecifications(BaseSpecifications<ApplicationUser> specifications)
        {
            return await ApplySpecifications(specifications).ToListAsync();
        }


        public IQueryable<ApplicationUser> ApplySpecifications(BaseSpecifications<ApplicationUser> specifications)
        {


            return BaseSpecificationsEvaluator<ApplicationUser>.GetQuery(userManager.Users, specifications);
        }


        public IQueryable<TResult> ApplyProjectionSpecifications<TResult>(ProjectionSpecifications<ApplicationUser, TResult> specifications)
        {


            return ProjectionSpecificationsEvaluator<ApplicationUser, TResult>.GetQuery(userManager.Users, specifications);
        }

        public async Task<ApplicationUser?> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task UpdateUser(ApplicationUser user)
        {


            await userManager.UpdateAsync(user);


        }
    }
}

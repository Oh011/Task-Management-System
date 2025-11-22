using Domain.Entities.IdentityModels;

namespace Domain.Contracts
{
    public interface IUserRepository
    {



        Task<ApplicationUser?> GetUserById(string id);



        Task<IEnumerable<ApplicationUser>> GetAllWithSpecifications(BaseSpecifications<ApplicationUser> specifications);



        Task UpdateUser(ApplicationUser user);


        Task<IEnumerable<TResult>> GetAllWithProjectionSpecifications<TResult>(ProjectionSpecifications<ApplicationUser, TResult> specifications);


    }
}

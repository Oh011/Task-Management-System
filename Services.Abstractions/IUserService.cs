using Microsoft.AspNetCore.Http;
using Shared.Dtos;
using Shared.ParameterTypes;

namespace Services.Abstractions
{
    public interface IUserService
    {


        Task<IEnumerable<UserInfoDto>> GetUsersAsync(UserSearchParameters parameters);



        Task<string> UploadProfileImage(IFormFile? file, string userId);
        Task<UserProfileDto> GetUserProfileAsync(string userId);
        Task<UserInfoDto> GetUserById(string userId);

    }
}

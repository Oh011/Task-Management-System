using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Services.Abstractions;
using Services.Specifications.User;
using Shared.Dtos;
using Shared.ParameterTypes;

namespace Services
{
    class UserService(IUserRepository userRepository, IMapper mapper, IImageUploadService imageUploadService) : IUserService
    {
        public async Task<UserInfoDto> GetUserById(string userId)
        {

            var user = await userRepository.GetUserById(userId);

            if (user == null || user.IsDeleted)
                throw new NotFoundException("User not found");


            return mapper.Map<UserInfoDto>(user);
        }



        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = (await userRepository.GetAllWithProjectionSpecifications(new
                UserProfileSpecifications(userId))).FirstOrDefault();

            if (user == null)
                throw new NotFoundException("User not found");



            return user;
        }

        public async Task<IEnumerable<UserInfoDto>> GetUsersAsync(UserSearchParameters parameters)
        {

            var users = await userRepository.GetAllWithProjectionSpecifications(new UserSpecifications(parameters));


            return users;

        }

        public async Task<string> UploadProfileImage(IFormFile? file, string userId)
        {

            if (file == null || file.Length == 0)
                throw new BadRequestException("No image file provided.");


            var user = await userRepository.GetUserById(userId);


            if (user == null)
                throw new NotFoundException("User not found");



            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {

                imageUploadService.DeleteFile(user.ProfileImageUrl);
            }

            var relativePtah = await imageUploadService.UploadFileAsync(file);


            user.ProfileImageUrl = relativePtah;


            await userRepository.UpdateUser(user);

            return user.ProfileImageUrl;
        }
    }
}

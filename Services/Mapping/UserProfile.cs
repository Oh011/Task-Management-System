using AutoMapper;
using Domain.Entities.IdentityModels;
using Shared.Dtos;

namespace Services.Mapping
{
    public class UserProfile : Profile
    {


        public UserProfile()
        {



            CreateMap<ApplicationUser, UserInfoDto>().ReverseMap();

            CreateMap<ApplicationUser, UserProfileDto>().ReverseMap();



        }
    }
}

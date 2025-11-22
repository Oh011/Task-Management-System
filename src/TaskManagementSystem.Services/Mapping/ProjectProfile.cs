using AutoMapper;
using Domain.Entities.ProjectModels;
using Shared.Dtos.Project;

namespace Services.Mapping
{
    public class ProjectProfile : Profile
    {



        public ProjectProfile()
        {




            CreateMap<Project, CreateProjectDto>().ReverseMap();


            CreateMap<Project, ProjectSummaryDto>().ReverseMap();


            CreateMap<Project, ProjectResultDto>().ReverseMap();



            CreateMap<Project, ProjectUpdateDto>().ReverseMap()
                  .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); ;

        }
    }
}

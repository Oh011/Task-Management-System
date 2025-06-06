using AutoMapper;
using Domain.Entities.TaskModels;
using Shared.Dtos.Tasks;

namespace Services.Mapping
{
    public class TaskProfile : Profile
    {

        public TaskProfile()
        {


            CreateMap<TaskItem, CreateProjectTaskDto>().ReverseMap();


            CreateMap<CreateTaskDto, TaskItem>()
                .ForMember(dest => dest.TaskType, opt => opt.Ignore());
            ;


            CreateMap<TaskDetailsDto, ProjectTaskDetailsDto>().ReverseMap();





            CreateMap<UpdateTaskDto, TaskItem>().ReverseMap();

            CreateMap<TaskResultDto, TaskItem>().ReverseMap();

            CreateMap<TaskResultDto, ProjectTaskResultDto>().ReverseMap();



            CreateMap<ProjectTaskResultDto, TaskItem>().ReverseMap();
            CreateMap<TaskDetailsDto, TaskItem>().ReverseMap();

        }
    }
}

//AutoMapper handles enum conversions automatically:

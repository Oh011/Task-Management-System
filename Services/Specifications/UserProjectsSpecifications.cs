using Domain.Contracts;

using Shared.ParameterTypes;
using Project = Domain.Entities.ProjectModels.Project;

namespace Services.Specifications
{
    internal class UserProjectsSpecifications : BaseSpecifications<Project>
    {





        public UserProjectsSpecifications(string id, ProjectParameters parameters) :
            base(project => (project.ProjectUsers != null && project.ProjectUsers.Any(u => u.UserId == id)) &&

       (string.IsNullOrEmpty(parameters.Name) || project.Name.ToLower().Contains(parameters.Name.ToLower())))
        {



            if (parameters.SortOptions != null)
            {



                switch (parameters.SortOptions)
                {


                    case ProjectSortOptions.StartDateAsc:
                        SetOrderBy(p => p.StartDate);
                        break;

                    case ProjectSortOptions.StartDateDesc:
                        SetOrderByDescending(p => p.StartDate);
                        break;

                    default:
                        SetOrderBy(p => p.Name ?? string.Empty); break;




                }


            }

            ApplyPagination(parameters.PageIndex, parameters.PageSize);
        }
    }
}

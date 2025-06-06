using Domain.Contracts;
using Domain.Entities.ProjectModels;
using Shared.ParameterTypes;

namespace Services.Specifications.ProjectSpecifications
{
    public class ProjectSpecifications : BaseSpecifications<Project>
    {


        public ProjectSpecifications(int id) : base(P => P.Id == id)
        {


        }






        public ProjectSpecifications(ProjectParameters parameters) :
            base(project =>

        string.IsNullOrEmpty(parameters.Name) || project.Name.ToLower().Contains(parameters.Name.ToLower()))
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

using Domain.Entities.TaskModels;

namespace Domain.Entities.ProjectModels
{
    public class ProjectTask : BaseEntity<int>
    {

        public int TaskItemId { get; set; }

        public virtual TaskItem TaskItems { get; set; }



        public int? ProjectId { get; set; }


        public Project? Project { get; set; }



    }
}

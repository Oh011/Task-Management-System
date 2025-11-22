namespace Domain.Exceptions
{
    public class ProjectNotFoundException : NotFoundException
    {
        public ProjectNotFoundException(int id) : base($"Project with Id :{id} is not found")
        {
        }
    }
}

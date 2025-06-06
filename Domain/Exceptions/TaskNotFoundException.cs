namespace Domain.Exceptions
{
    public class TaskNotFoundException : NotFoundException
    {
        public TaskNotFoundException(int id) : base($"The Task with Id : {id} is not found")
        {
        }
    }
}

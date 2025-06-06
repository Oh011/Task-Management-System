namespace Domain.Exceptions
{
    public class ProjectUnauthorizedActionException : ForbiddenException
    {
        public ProjectUnauthorizedActionException() : base("You are not authorized to perform this action.")
        {
        }
    }
}

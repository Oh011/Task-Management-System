namespace Domain.Exceptions
{
    public class UserNameExists : ResourceExists
    {

        public UserNameExists(string username) : base($"User Name :{username} already exists !") { }
    }
}

namespace Domain.Exceptions
{
    public class UnAuthorizedException : Exception
    {

        public UnAuthorizedException(string msg = "Invalid email or password") : base(msg) { }
    }
}

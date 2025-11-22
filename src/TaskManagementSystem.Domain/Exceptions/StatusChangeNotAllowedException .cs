namespace Domain.Exceptions
{
    public class StatusChangeNotAllowedException : Exception
    {
        public StatusChangeNotAllowedException(string message) : base(message) { }
    }
}

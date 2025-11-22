namespace Domain.Exceptions
{
    public class InvalidInvitationOperationException : BadRequestException
    {
        public InvalidInvitationOperationException(string message) : base(message)
        {
        }
    }
}

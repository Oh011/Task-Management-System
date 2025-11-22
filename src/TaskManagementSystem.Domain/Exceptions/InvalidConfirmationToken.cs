namespace Domain.Exceptions
{
    public class InvalidConfirmationToken : BadRequestException
    {


        public InvalidConfirmationToken(string msg) : base(msg) { }
    }
}

namespace Domain.Contracts
{
    public interface ILinkBuilder
    {

        string BuildEmailConfirmationLink(string token, string email);
        string BuildPasswordResetLink(string token, string email);
    }
}

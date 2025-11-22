namespace Domain.Exceptions
{
    public class AccountLockedException : Exception
    {

        public AccountLockedException() : base("Account locked due to multiple failed login attempts. Try again later.") { }
    }
}

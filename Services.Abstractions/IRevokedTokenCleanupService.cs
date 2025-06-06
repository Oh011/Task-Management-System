namespace Services.Abstractions
{
    public interface IRevokedTokenCleanupService
    {


        Task CleanUpAsync();
    }
}

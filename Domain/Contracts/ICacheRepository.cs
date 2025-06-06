namespace Domain.Contracts
{
    public interface ICacheRepository
    {



        void set(string key, object value, TimeSpan TTL);


        T GetOrAdd<T>(string key, Func<T> acquire, TimeSpan ttl);
        object? Get(string key);
    }
}

namespace Services.Abstractions
{


    public interface IProjectCleanupService
    {


        Task CleanupProjectTasksAsync(List<int> taskIds);

    }


}

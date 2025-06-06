namespace Shared.ParameterTypes
{
    public class UserSearchParameters : PaginationQueryParameters
    {
        public string? Query { get; set; }  // This can contain either part of username or email
    }
}

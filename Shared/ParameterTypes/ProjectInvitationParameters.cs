using Domain.Entities;

namespace Shared.ParameterTypes
{
    public class ProjectInvitationParameters : PaginationQueryParameters
    {

        public InvitationStatus? Status { get; set; } // Optional filter
        public bool SortBySentAtAsc { get; set; } = false; // Sort direction
    }
}

namespace Shared.Dtos.Invitations
{
    public class UserProjectInvitationsDto
    {

        public int InvitationId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }

        public string status { get; set; }
    }
}

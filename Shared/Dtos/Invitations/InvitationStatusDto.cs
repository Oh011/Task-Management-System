using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos.Invitations
{
    public class InvitationStatusDto
    {
        [Required]
        public InvitationStatus Status { get; set; }
    }
}

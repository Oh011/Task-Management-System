using Domain.Entities.TaskModels;

namespace Domain.Entities.IdentityModels
{
    public class RefreshToken : BaseEntity<int>
    {


        public string UserId { get; set; }  // Link to ApplicationUser


        public string DeviceId { get; set; }  // New field to store DeviceId
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Revoked { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        public ApplicationUser User { get; set; }  // Navigation property
    }

}

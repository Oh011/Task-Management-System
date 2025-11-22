namespace Shared.Dtos.Identity
{
    public class LogInResultDto : UserResultDto
    {

        public string RefreshToken { get; set; }
    }
}

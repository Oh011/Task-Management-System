
namespace Services.Abstractions
{
    public interface IServiceManager
    {



        ITaskService TaskService { get; }

        IProjectService ProjectService { get; }


        IRegistrationService RegistrationService { get; }


        IAuthenticationService AuthenticationService { get; }
        IRefreshTokenService RefreshTokenService { get; }


        IExternalLoginService ExternalLoginService { get; }


        ITokenService TokenService { get; }

        IEmailService EmailService { get; }


        IProjectTasksService ProjectTasksService { get; }

        IProjectAuthorizationService ProjectAuthorizationService { get; }

        IUserProjectService UserProjectService { get; }


        IStatusTransitionService StatusTransitionService { get; }


        IProjectInvitationService ProjectInvitationService { get; }



        IImageUploadService ImageUploadService { get; }

        IProjectDashboardService ProjectDashboardService { get; }

        ITaskAuthorizationService TaskAuthorizationService { get; }
        IUserService UserService { get; }
    }
}
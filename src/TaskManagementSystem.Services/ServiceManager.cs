using AutoMapper;
using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Services.Abstractions.Factories;
using Shared.Options;

namespace Services
{
    public class ServiceManager : IServiceManager
    {


        private readonly Lazy<ITaskService> _taskService;

        private readonly Lazy<IProjectService> _ProjectService;

        private readonly Lazy<IEmailService> _EmailService;


        private readonly Lazy<IRegistrationService> _RegistrationService;


        private readonly Lazy<IAuthenticationService> _AuthenticationService;


        private readonly Lazy<ITokenService> _TokenService;


        private readonly Lazy<IRefreshTokenService> _RefreshTokenService;


        private readonly Lazy<IExternalLoginService> _ExternalLoginService;


        private readonly Lazy<IProjectTasksService> _ProjectTasksService;


        private readonly Lazy<IUserProjectService> _UserProjectService;


        private readonly Lazy<IStatusTransitionService> _StatusTransitionService;


        private readonly Lazy<IProjectAuthorizationService> _ProjectAuthorizationService;


        private readonly Lazy<IUserService> _userService;

        private readonly Lazy<IProjectInvitationService> _invitationService;


        private readonly Lazy<ITaskAuthorizationService> _taskAuthorizationService;


        private readonly Lazy<IProjectDashboardService> _projectDashboardService;


        private readonly Lazy<IImageUploadService> _imageUploadService;


        public ServiceManager(ILinkBuilder linkBuilder, SignInManager<ApplicationUser> signInManager,
            IOptions<SmtpOptions> SmtpOptions, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork,
            IMapper mapper, ICacheRepository cacheRepository, IOptions<JwtOptions> options, IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository, IAuthorizationRequirementFactory authorizationRequirementFactory,
            IBackgroundJobClient backgroundJobClient, IProjectCleanupService projectCleanupService, IOptions<ImageOptions> imageOptions
            )
        {

            _imageUploadService = new Lazy<IImageUploadService>(() => new ImageUploadService(imageOptions));

            _projectDashboardService = new Lazy<IProjectDashboardService>(() => new ProjectDashboardService(this.TaskService, this.ProjectService, this.UserProjectService));

            _taskAuthorizationService = new Lazy<ITaskAuthorizationService>(() => new TaskAuthorizationService());

            _invitationService = new Lazy<IProjectInvitationService>(() => new ProjectInvitationService(this.UserProjectService, this.ProjectAuthorizationService, unitOfWork));

            _userService = new Lazy<IUserService>(() => new UserService(userRepository, mapper, this.ImageUploadService));

            _ProjectAuthorizationService = new Lazy<IProjectAuthorizationService>(() => new ProjectAuthorizationService(this.TaskService, unitOfWork, authorizationRequirementFactory));

            _StatusTransitionService = new Lazy<IStatusTransitionService>(() => new StatusTransitionService(cacheRepository));
            _UserProjectService = new Lazy<IUserProjectService>(() => new UserProjectService(unitOfWork, userRepository, this.ProjectAuthorizationService));

            _ProjectTasksService = new Lazy<IProjectTasksService>(() => new ProjectTasksService(unitOfWork, this.UserService, this.UserProjectService, this.ProjectService, this.TaskService, this.ProjectAuthorizationService, mapper));

            _ExternalLoginService = new Lazy<IExternalLoginService>(() => new ExternalLoginService(userManager, this.TokenService, this.RefreshTokenService));

            _RefreshTokenService = new Lazy<IRefreshTokenService>(() => new RefreshTokenService(unitOfWork, httpContextAccessor, this.TokenService, userManager));

            _AuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, signInManager, this.TokenService, linkBuilder, this.EmailService, this.RefreshTokenService));

            _TokenService = new Lazy<ITokenService>(() => new TokenService(options, userManager));

            _EmailService = new Lazy<IEmailService>(() => new EmailService(SmtpOptions));

            _RegistrationService = new Lazy<IRegistrationService>(() => new RegistrationService(userManager, _EmailService.Value, linkBuilder, backgroundJobClient));

            _taskService = new Lazy<ITaskService>(() => new TaskService(unitOfWork, mapper, this.StatusTransitionService, this.TaskAuthorizationService));

            _ProjectService = new Lazy<IProjectService>(() => new ProjectService(unitOfWork, mapper, this.ProjectAuthorizationService, this.StatusTransitionService, this.UserProjectService, backgroundJobClient, projectCleanupService));
        }
        public ITaskService TaskService => _taskService.Value;

        public IProjectService ProjectService => _ProjectService.Value;

        public IRegistrationService RegistrationService => _RegistrationService.Value;

        public IEmailService EmailService => _EmailService.Value;

        public IAuthenticationService AuthenticationService => _AuthenticationService.Value;

        public ITokenService TokenService => _TokenService.Value;

        public IRefreshTokenService RefreshTokenService => _RefreshTokenService.Value;

        public IExternalLoginService ExternalLoginService => _ExternalLoginService.Value;

        public IProjectTasksService ProjectTasksService => _ProjectTasksService.Value;

        public IUserProjectService UserProjectService => _UserProjectService.Value;

        public IStatusTransitionService StatusTransitionService => _StatusTransitionService.Value;

        public IProjectAuthorizationService ProjectAuthorizationService => _ProjectAuthorizationService.Value;

        public IUserService UserService => _userService.Value;

        public IProjectInvitationService ProjectInvitationService => _invitationService.Value;

        public ITaskAuthorizationService TaskAuthorizationService => _taskAuthorizationService.Value;

        public IProjectDashboardService ProjectDashboardService => _projectDashboardService.Value;

        public IImageUploadService ImageUploadService => _imageUploadService.Value;
    }
}

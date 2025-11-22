using Domain.Contracts;
using Domain.Entities.IdentityModels;
using Domain.Exceptions;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Services.Abstractions;
using Shared;
using Shared.Dtos.Identity;


namespace Services
{
    internal class RegistrationService : IRegistrationService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILinkBuilder _linkBuilder;

        private readonly IEmailService _emailService;

        private readonly IBackgroundJobClient backgroundJobClient;


        public RegistrationService(UserManager<ApplicationUser> userManager, IEmailService emailService, ILinkBuilder emailLinkBuilder, IBackgroundJobClient backgroundJobClient)
        {
            _userManager = userManager;
            _emailService = emailService;
            _linkBuilder = emailLinkBuilder;
            this.backgroundJobClient = backgroundJobClient;
        }



        public async Task<RegistrationResponseDto> Register(UserRegisterDto userRegisterDto)
        {


            var user = new ApplicationUser()
            {

                FullName = userRegisterDto.FullName,
                Email = userRegisterDto.Email,
                UserName = userRegisterDto.UserName,

            };

            user.EmailConfirmed = true;

            var ExistingUserName = await _userManager.FindByNameAsync(user.UserName);

            if (ExistingUserName != null)
            {
                throw new UserNameExists(userRegisterDto.UserName);

            }


            var Result = await _userManager.CreateAsync(user, userRegisterDto.Password);



            if (!Result.Succeeded)
            {

                var errors = Result.Errors
                            .GroupBy(e => e.Code)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToList());

                throw new ValidationException(errors);

            }

            await _userManager.AddToRoleAsync(user, "User");


            var Token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //This method enables two-factor authentication (2FA) for a user.

            //Two-factor authentication (2FA) is a separate security feature that requires users to
            //confirm their identity using a second step during login (like an SMS code or an authenticator app).
            //await _userManager.SetTwoFactorEnabledAsync(user, true);



            var confirmationLink = _linkBuilder.BuildEmailConfirmationLink(Token, user.Email);



            var email = new Email
            {
                To = userRegisterDto.Email,
                Subject = "Confirm your email address",
                Body = $"Please confirm your email by clicking on the following link: {confirmationLink}"
            };


            //await _emailService.SendEmailAsync(email);


            backgroundJobClient.Enqueue<IEmailService>(emailService =>
    emailService.SendEmailAsync(email));





            return new RegistrationResponseDto(user.FullName, "Registration successful. Please check your email to confirm your account.", user.Email);
        }







        public async Task<string> ConfirmEmail(EmailConfirmDto dto)
        {


            var user = await _userManager.FindByEmailAsync(dto.Email);



            if (user == null)
            {
                throw new NotFoundException($"No user found with email: {dto.Email}");
            }





            var result = await _userManager.ConfirmEmailAsync(user, dto.Token);

            if (!result.Succeeded)
            {

                throw new InvalidConfirmationToken("Email Confirmation Token is invalid ");
            }


            return "Email confirmed successfully.";
        }





    }
}




//when signInManger is needed:

//managing cookies or sessions that SignInManager would typically help with,
//which is common for traditional authentication scenarios. Instead, you're creating
//a token directly and returning it to the user.

//-->var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

//SignInManager is usually used for handling sign-ins with additional features like multi - factor
//authentication, external logins(e.g., Google, Facebook), or adding cookies for local sign-in. 
//It can be more useful when you are dealing with cookie - based authentication or need complex 
//sign -in behavior.

//In your case, you're focusing on JWT authentication, which is token-based and doesn't rely on 
//cookies or session management that SignInManager typically helps with.


//-----------------------------------------------------------------------------


//it's generally recommended to authenticate the user immediately after registration and issue a JWT token
//at that point.

//✅ Recommended Approach: Auto - Authenticate After Registration
//Why?
//Enhanced User Experience: Automatically logging in users after registration eliminates the need
//for them to log in again, providing a smoother

//---------------------------------------------------------


//The JWT signature is hashed, not encrypted.

//Here’s a breakdown:

//🔐 Signature = Hashed with HMAC or RSA
//In most common scenarios, the signature is created using:

//HMAC - SHA256(symmetric, shared secret key)
//or RSA-SHA256 (asymmetric, public/private key


//----------------------------------------------------------

//some Improvements on this class :

//🔹 2. Extract GetConfirmEmailPath into a Service
//The method GetConfirmEmailPath(...) is a separate concern. Move it to its own service.


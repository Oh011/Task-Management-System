using FluentValidation;
using Shared.Dtos.Identity;

namespace Services.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {

        public UserRegisterDtoValidator()
        {



            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required.");

            RuleFor(x => x.ConfirmPassword).Matches(x => x.Password).WithMessage("Confirm Password does not match Password");

        }
    }
}

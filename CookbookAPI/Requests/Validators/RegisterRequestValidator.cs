using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Requests.Account;
using FluentValidation;

namespace CookbookAPI.Requests.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(CookbookDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken");
                    }
                });

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("'{PropertyName}' must contain one or more capital letters.")
                .Matches("[a-z]").WithMessage("'{PropertyName}' must contain one or more lowercase letters.")
                .Matches(@"\d").WithMessage("'{PropertyName}' must contain one or more digits.")
                .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]")
                .WithMessage("'{PropertyName}' must contain one or more special characters.")
                .Matches("^[^£# “”]*$")
                .WithMessage("'{PropertyName}' must not contain the following characters £ # “” or spaces.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(e => e.Password).WithMessage("'{PropertyName}' must match 'Password'");

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}

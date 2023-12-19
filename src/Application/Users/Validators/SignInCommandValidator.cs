using EvaExchange.API.Application.Users.Commands;
using FluentValidation;

namespace EvaExchange.API.Application.Users.Validators;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
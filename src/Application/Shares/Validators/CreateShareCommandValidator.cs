using EvaExchange.API.Application.Shares.Commands;
using FluentValidation;

namespace EvaExchange.API.Application.Shares.Validators;

public class CreateShareCommandValidator : AbstractValidator<CreateShareCommand>
{
    public CreateShareCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().MaximumLength(3).Matches("^[A-Z]+$")
            .WithMessage($"The '{nameof(CreateShareCommand.Id)}' must be only capital letters.");
        RuleFor(x => x.Rate).NotNull().GreaterThan(0).LessThan(100);
        RuleFor(x => x.Price).NotNull().GreaterThan(0);
    }
}

using EvaExchange.API.Application.Shares.Commands;
using FluentValidation;

namespace EvaExchange.API.Application.Shares.Validators;

public class UpdateShareCommandValidator : AbstractValidator<UpdateShareCommand>
{
    public UpdateShareCommandValidator()
    {
        RuleFor(x => x.Price).NotNull().GreaterThan(0);
    }
}

using EvaExchange.API.Application.Trades.Commands;
using FluentValidation;

namespace EvaExchange.API.Application.Trades.Validators;

public class SellCommandValidator : AbstractValidator<SellCommand>
{
    public SellCommandValidator()
    {
        RuleFor(x => x.Rate).NotNull().GreaterThan(0).LessThan(100);
    }
}
using EvaExchange.API.Application.Trades.Commands;
using FluentValidation;

namespace EvaExchange.API.Application.Trades.Validators;

public class BuyCommandValidator : AbstractValidator<BuyCommand>
{
    public BuyCommandValidator()
    {
        RuleFor(x => x.Rate).NotNull().GreaterThan(0).LessThan(100);
    }
}
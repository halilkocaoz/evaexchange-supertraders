using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Infrastructure.Models;
using EvaExchange.API.Infrastructure.Services;
using MediatR;

namespace EvaExchange.API.Application.Users.Commands;

public record SignInCommand(string Email, string Password) : IRequest<TokenModel>;

public class SignInCommandHandler(IUserRepository userRepository, ITokenService tokenService) : IRequestHandler<SignInCommand, TokenModel>
{
    public async Task<TokenModel> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return null;

        var passwordMatches = user.PasswordMatches(request.Password);
        return passwordMatches ? tokenService.GenerateToken(user) : null;
    }
}
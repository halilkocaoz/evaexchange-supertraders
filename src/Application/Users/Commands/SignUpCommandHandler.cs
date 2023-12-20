using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Infrastructure;
using EvaExchange.API.Infrastructure.Models;
using EvaExchange.API.Infrastructure.Services;
using MediatR;

namespace EvaExchange.API.Application.Users.Commands;

public record SignUpCommand(string Email, string Password, string FullName) : IRequest<TokenModel>;

public class SignUpCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    : IRequestHandler<SignUpCommand, TokenModel>
{
    public async Task<TokenModel> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);
        if (user != null)
            throw new ApiException(400, $"User with the Email {request.Email} already exists.");
        
        user = new User(request.Email, request.Password, request.FullName);
        await userRepository.AddAsync(user);
        return tokenService.GenerateToken(user);
    }
}
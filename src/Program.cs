using System.Text;
using EvaExchange.API.Application.Shares.Commands;
using EvaExchange.API.Application.Shares.Validators;
using EvaExchange.API.Application.Trades.Commands;
using EvaExchange.API.Application.Trades.Validators;
using EvaExchange.API.Application.Users.Commands;
using EvaExchange.API.Application.Users.Validators;
using EvaExchange.API.Data;
using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Infrastructure;
using EvaExchange.API.Infrastructure.Application;
using EvaExchange.API.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

AddMediatR();
AddInjections();
AddAuthentication();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();
await SeedAsync(dbContext);

app.Run();
return;

void AddMediatR()
{
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
        cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    });
    builder.Services.AddSingleton<IValidator<CreateShareCommand>, CreateShareCommandValidator>();
    builder.Services.AddSingleton<IValidator<UpdateShareCommand>, UpdateShareCommandValidator>();
    builder.Services.AddSingleton<IValidator<SignInCommand>, SignInCommandValidator>();
    builder.Services.AddSingleton<IValidator<SignUpCommand>, SignUpCommandValidator>();
    builder.Services.AddSingleton<IValidator<SellCommand>, SellCommandValidator>();
    builder.Services.AddSingleton<IValidator<BuyCommand>, BuyCommandValidator>();
}

void AddInjections()
{
    builder.Services.AddScoped<IShareRepository, ShareRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserShareRepository, UserShareRepository>();
    builder.Services.AddScoped<ITradeOperations, TradeOperations>();

    builder.Services.AddSingleton<ITokenService, TokenService>();
}

void AddAuthentication()
{
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateIssuer = false,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ??
                                                                               throw new ArgumentNullException(
                                                                                   "Jwt:Key is missing"))),
            ClockSkew = TimeSpan.Zero
        };
    });
    builder.Services.AddHttpContextAccessor();
}

Task SeedAsync(AppDbContext context)
{
    var testUser = new User("test@test.com", "test", "Test User");
    var evaFounderUser = new User("founder@eva.guru", "pass123*", "Eva Founder");

    if (context.Users.Any() is false)
    {
        context.Users.AddRange(testUser, evaFounderUser);

        context.Shares.AddRange(new List<Share>
        {
            new("TST", 49.50m, 100, testUser.Id),
            new("EVA", 25.75m, 225.15m, evaFounderUser.Id),
        });

        context.UserShares.AddRange(new List<UserShares>
        {
            new()
            {
                UserId = testUser.Id,
                ShareId = "EVA",
                Rate = 1.5m,
            },
            new()
            {
                UserId = evaFounderUser.Id,
                ShareId = "TST",
                Rate = 3.3m,
            }
        });
    }

    return context.SaveChangesAsync();
}
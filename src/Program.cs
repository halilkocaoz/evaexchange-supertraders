using EvaExchange.API.Application.Shares.Commands;
using EvaExchange.API.Application.Shares.Validators;
using EvaExchange.API.Data;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Infrastructure;
using EvaExchange.API.Infrastructure.Application;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
});
builder.Services.AddScoped<IShareRepository, ShareRepository>();
builder.Services.AddSingleton<IValidator<CreateShareCommand>, CreateShareCommandValidator>();
builder.Services.AddSingleton<IValidator<UpdateShareCommand>, UpdateShareCommandValidator>();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();

app.Run();

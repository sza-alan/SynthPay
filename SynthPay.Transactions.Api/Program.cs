using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SynthPay.Transactions.Api.Middlewares;
using SynthPay.Transactions.Application.Behaviors;
using SynthPay.Transactions.Application.Interfaces;
using SynthPay.Transactions.Infrastructure.BackgroundJobs;
using SynthPay.Transactions.Infrastructure.Persistence;
using SynthPay.Transactions.Infrastructure.Repositories;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ITransactionRepository).Assembly);

    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(ITransactionRepository).Assembly);

builder.Services.AddDbContext<SynthPayDbContext>(options => options.UseSqlite("Data Source=synthpay.db"));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddHostedService<OutboxProcessorBackgroundService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ITransactionRepository).Assembly));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
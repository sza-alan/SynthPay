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

builder.Services.AddDbContext<SynthPayDbContext>(options =>
    options.UseSqlite("Data Source=synthpay.db",
        b => b.MigrationsAssembly("SynthPay.Transactions.Infrastructure")));

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddHostedService<OutboxProcessorBackgroundService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ITransactionRepository).Assembly));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = builder.Configuration["RabbitMqHost"] ?? "localhost";

        cfg.Host(rabbitMqHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SynthPay.Transactions.Infrastructure.Persistence.SynthPayDbContext>();

    db.Database.EnsureDeleted();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
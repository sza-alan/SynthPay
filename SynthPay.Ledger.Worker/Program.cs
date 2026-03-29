using MassTransit;
using Microsoft.EntityFrameworkCore;
using SynthPay.Ledger.Worker.Consumers;
using SynthPay.Ledger.Worker.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<LedgerDbContext>(options =>
    options.UseSqlite("Data Source=ledger.db"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TransactionCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("ledger-transaction-created-queue", e =>
        {
            e.ConfigureConsumer<TransactionCreatedEventConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();
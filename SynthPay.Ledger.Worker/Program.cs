using MassTransit;
using Microsoft.EntityFrameworkCore;
using SynthPay.Ledger.Worker.Consumers;
using SynthPay.Ledger.Worker.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LedgerDbContext>(options =>
    options.UseSqlite("Data Source=ledger.db",
        b => b.MigrationsAssembly("SynthPay.Ledger.Worker")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TransactionCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = builder.Configuration["RabbitMqHost"] ?? "localhost";

        cfg.Host(rabbitMqHost, "/", h => {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("ledger-transaction-created-queue", e =>
        {
            e.ConfigureConsumer<TransactionCreatedEventConsumer>(context);
        });
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SynthPay.Ledger.Worker.Infrastructure.Persistence.LedgerDbContext>();

    db.Database.EnsureDeleted();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
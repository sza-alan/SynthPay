using MediatR;
using SynthPay.Transactions.Domain.Enums;

namespace SynthPay.Transactions.Application.Commands
{
    public record CreateTransactionCommand(
        Guid AccountId,
        decimal Amount,
        TransactionType Type) : IRequest<Guid>;
}

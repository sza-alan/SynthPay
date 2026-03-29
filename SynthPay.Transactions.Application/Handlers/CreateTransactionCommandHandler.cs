using MediatR;
using SynthPay.Transactions.Application.Commands;
using SynthPay.Transactions.Application.Interfaces;
using SynthPay.Transactions.Domain.Entities;

namespace SynthPay.Transactions.Application.Handlers
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly ITransactionRepository _repository;

        public CreateTransactionCommandHandler(ITransactionRepository repository) => _repository = repository;

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction(request.AccountId, request.Amount, request.Type);

            await _repository.AddAsync(transaction, cancellationToken);

            return transaction.Id;
        }
    }
}
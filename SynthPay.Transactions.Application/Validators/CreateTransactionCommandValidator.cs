using FluentValidation;
using SynthPay.Transactions.Application.Commands;

namespace SynthPay.Transactions.Application.Validators
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("O ID da conta é obrigatório e não pode ser vazio.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Tipo de transação inválido. Use 1 para Crédito ou 2 para Débito.");
        }
    }
}

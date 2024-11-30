using System;
using FluentValidation;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;

namespace TrackIT.Api.Data.Validation;

public class TransactionValidator : AbstractValidator<Transaction>
{
    public TransactionValidator(){
        RuleFor(transaction => transaction.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.")
            .PrecisionScale(18, 2, true)
            .WithMessage("Amount can have up to 18 digits with 2 decimal places.");

        RuleFor(transaction => transaction.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Date cannot be in the future.");

        RuleFor(transaction => transaction.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be a positive number.");

        RuleFor(transaction => transaction.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(transaction => !string.IsNullOrEmpty(transaction.Description));
    }
}

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionDtoValidator(){
        RuleFor(transaction => transaction.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.")
            .PrecisionScale(18, 2, true)
            .WithMessage("Amount can have up to 18 digits with 2 decimal places.");

        RuleFor(transaction => transaction.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Date cannot be in the future.");

        RuleFor(transaction => transaction.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be a positive number.");

        RuleFor(transaction => transaction.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(transaction => !string.IsNullOrEmpty(transaction.Description));
    }
}
public class UpdateTransactionDtoValidator : AbstractValidator<UpdateTransactionDto>
{
    public UpdateTransactionDtoValidator(){
        RuleFor(transaction => transaction.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.")
            .PrecisionScale(18, 2, true)
            .WithMessage("Amount can have up to 18 digits with 2 decimal places.");

        RuleFor(transaction => transaction.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Date cannot be in the future.");

        RuleFor(transaction => transaction.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be a positive number.");

        RuleFor(transaction => transaction.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(transaction => !string.IsNullOrEmpty(transaction.Description));
    }
}



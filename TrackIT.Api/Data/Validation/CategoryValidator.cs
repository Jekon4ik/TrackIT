using System;

using FluentValidation;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;

namespace TrackIT.Api.Data.Validation;

/*
public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
         RuleFor(category => category.Name)
            .NotEmpty().WithMessage("Name of Category is required.")
            .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");

        RuleFor(category => category.TypeId)
            .GreaterThan(0).WithMessage("TypeId must be greater than 0.");
    }
}

*/


public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");

        RuleFor(dto => dto.TypeId)
            .GreaterThan(0).WithMessage("TypeId must be greater than 0.");
    }
}

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");

        RuleFor(dto => dto.TypeId)
            .GreaterThan(0).WithMessage("TypeId must be greater than 0.");
    }
}


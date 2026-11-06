using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DTOs;

namespace ProductService.Validators;

public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
{
    private readonly AppDbContext _context;
    public CreateProductDtoValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative.");

        RuleFor(x => x.Name)
            .Must(name => name.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
            .WithMessage("Product name can only contain letters, numbers, and spaces.");

        When(x => x.StockQuantity == 0, () =>
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required when product is out of stock.");
        });

        RuleFor(x => x.Name) 
            .MustAsync(async (name, cancellationToken) =>
            {
                return !await _context.Products.AnyAsync(p => p.Name == name, cancellationToken);
            }).WithMessage("Product name already exists.");
    }
}
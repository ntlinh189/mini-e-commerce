namespace ProductService.Exceptions;

public class DuplicateProductNameException : DomainException
{
    public DuplicateProductNameException(string productName)
        : base($"Product name '{productName}' already exists.")
    {
    }
}
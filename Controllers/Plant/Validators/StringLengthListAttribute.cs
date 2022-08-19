using System.ComponentModel.DataAnnotations;

namespace Ipe.Controllers.Plant.Validators;

public class StringLengthListAttribute : ValidationAttribute
{
    private int MinLength { get; }
    private int MaxLength { get; }

    public StringLengthListAttribute(int minimumLength, int maximumLength) : base()
    {
        MinLength = minimumLength;
        MaxLength = maximumLength;
    }

    public override bool IsValid(object? value)
    {
        bool isValid = true;
        if (value is not null)
        {
            var List = value as List<string>;
            isValid = !List.Any(item => InvalidLenght(item.Length));
        }

        return isValid;
    }

    private bool InvalidLenght(int length)
        => length > MaxLength || length < MinLength;
}
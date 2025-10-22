using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BlazorMultiSelect.Validation;

public class AtLeastOneElementAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is ICollection { Count: > 0 };
    }
}

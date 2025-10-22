using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorMultiSelect.Components.Pages;

public partial class MultiselectComponent<TItem> : InputBase<List<TItem>>
{
    [Parameter, EditorRequired] public List<TItem> Data { get; set; } = [];
    [Parameter, EditorRequired] public string TextField { get; set; }
    [Parameter, EditorRequired] public string ValueField { get; set; }

    protected override bool TryParseValueFromString(string? value, out List<TItem> result, out string? validationErrorMessage)
    {
        result = [];
        validationErrorMessage = null;
        return true;
    }

    private string GetDisplayValue(TItem item)
    {
        // TODO: replace with funcs
        return item?.GetType().GetProperty(TextField)?.GetValue(item)?.ToString() ?? string.Empty;
    }

    private string GetValue(TItem item)
    {
        return item?.GetType().GetProperty(ValueField)?.GetValue(item)?.ToString() ?? string.Empty;
    }

    private void Onchange(ChangeEventArgs args)
    {
        var selectedOptions = (IEnumerable<TItem>)args.Value;
        var newSelectedItems = Data.Where(i => selectedOptions.Contains(i)).ToList();

        CurrentValue = newSelectedItems;
    }

    private void Test()
    {

    }

    private void OnCheckboxChanged(TItem value, object? checkedValue)
    {
        bool isChecked = checkedValue as bool? ?? checkedValue?.ToString() == "true";

        var current = CurrentValue?.ToList() ?? new List<TItem>();

        if (isChecked)
        {
            if (!current.Contains(value))
                current.Add(value);
        }
        else
        {
            current.Remove(value);
        }

        // Always assign a new list to trigger change notification
        CurrentValue = new List<TItem>(current);
    }

    private void Toggle(TItem item)
    {
        var current = CurrentValue?.ToList() ?? new List<TItem>();
        if (current.Contains(item)) current.Remove(item);
        else current.Add(item);
        CurrentValue = new List<TItem>(current);
    }

    private bool IsChecked(TItem value)
        => CurrentValue?.Contains(value) ?? false;
}

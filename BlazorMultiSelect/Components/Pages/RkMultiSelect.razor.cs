using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace BlazorMultiSelect.Components.Pages;

public partial class RkMultiSelect<TItem> : InputBase<List<TItem>>, IAsyncDisposable
{
    [Inject] public required IJSRuntime JsRuntime { get; set; }

    [Parameter, EditorRequired] public string Label { get; set; } = string.Empty;
    [Parameter, EditorRequired] public string Id { get; set; } = string.Empty;
    [Parameter] public bool IsRequired { get; set; } = false;
    [Parameter] public bool AutoFocus { get; set; } = false;
    [Parameter, EditorRequired] public List<TItem> Items { get; set; } = [];
    [Parameter, EditorRequired] public required Func<TItem, int> ValueSelector { get; set; }
    [Parameter, EditorRequired] public required Func<TItem, string> DisplayTextSelector { get; set; }

    private ElementReference _multiSelectElementReference;
    private DotNetObjectReference<RkMultiSelect<TItem>>? _multiSelectDotnetReference;
    private string SelectedItemsDisplayText { get; set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _multiSelectDotnetReference = DotNetObjectReference.Create(this);
            await JsRuntime.InvokeVoidAsync("multiSelect.init", _multiSelectElementReference, _multiSelectDotnetReference);
            if (AutoFocus)
            {
                await _multiSelectElementReference.FocusAsync();
            }

            UpdateDisplayText();
            StateHasChanged();
        }
    }

    protected override bool TryParseValueFromString(string? value, out List<TItem> result, out string validationErrorMessage)
    {
        result = [];
        validationErrorMessage = string.Empty;
        return true;
    }

    [JSInvokable]
    public void OnCheckBoxChanged(bool isChecked, int optionValue)
    {
        var value = Items.SingleOrDefault(x => ValueSelector(x) == optionValue);
        if (value is null) return;
        UpdateSelection(isChecked, value);
        StateHasChanged();
    }

    private void OnCheckboxChanged(ChangeEventArgs args, TItem value)
    {
        if (args.Value is null) return;
        UpdateSelection(Convert.ToBoolean(args.Value), value);
    }

    private void UpdateSelection(bool isChecked, TItem value)
    {
        var currentValue = CurrentValue?.ToList() ?? [];

        if (isChecked)
        {
            if (!currentValue.Contains(value))
                currentValue.Add(value);
        }
        else
        {
            currentValue.Remove(value);
        }

        CurrentValue = currentValue;

        UpdateDisplayText();
    }

    private void UpdateDisplayText()
    {
        if (CurrentValue is null) return;
        SelectedItemsDisplayText = string.Join(", ", CurrentValue.Select(x => DisplayTextSelector(x)).OrderBy(x => x).ToList());
    }

    private bool IsChecked(TItem value)
        => CurrentValue?.Contains(value) ?? false;

    public async ValueTask DisposeAsync()
    {
        await JsRuntime.InvokeVoidAsync("multiSelect.cleanup"); // TODO: remove eventhandlers
        _multiSelectDotnetReference?.Dispose();
    }
}

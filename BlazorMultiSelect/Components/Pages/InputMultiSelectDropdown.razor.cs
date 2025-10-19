// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Web;
//
// namespace BlazorMultiSelect.Components.Pages;
//
// public partial class InputMultiSelectDropdown : ComponentBase
// {
//     [Parameter] public List<TItem> Items { get; set; } = new();
//     [Parameter] public string Id { get; set; } = $"multiSelect_{Guid.NewGuid()}";
//     [Parameter] public string Placeholder { get; set; } = "Select...";
//
//     [Parameter, EditorRequired] public Func<TItem, TValue> ValueSelector { get; set; } = default!;
//     [Parameter, EditorRequired] public Func<TItem, string> TextSelector { get; set; } = default!;
//
//     private bool isOpen;
//     private int focusedIndex = -1;
//     private string AriaMessage = "";
//
//     private string ButtonText =>
//         CurrentValue?.Count > 0
//             ? string.Join(", ", Items.Where(i => CurrentValue.Contains((TValue)(object)ValueSelector(i)))
//                                      .Select(i => TextSelector(i)))
//             : Placeholder;
//
//     private void ToggleDropdown()
//     {
//         isOpen = !isOpen;
//         if (isOpen)
//             focusedIndex = 0;
//     }
//
//     private void HandleButtonKeydown(KeyboardEventArgs e)
//     {
//         if (e.Key is "ArrowDown" or "Enter" or " ")
//         {
//             e.PreventDefault();
//             isOpen = true;
//             focusedIndex = 0;
//         }
//     }
//
//     private async Task HandleListKeydown(KeyboardEventArgs e)
//     {
//         switch (e.Key)
//         {
//             case "ArrowDown":
//                 e.PreventDefault();
//                 focusedIndex = (focusedIndex + 1) % Items.Count;
//                 break;
//             case "ArrowUp":
//                 e.PreventDefault();
//                 focusedIndex = (focusedIndex - 1 + Items.Count) % Items.Count;
//                 break;
//             case " ":
//             case "Enter":
//                 e.PreventDefault();
//                 if (focusedIndex >= 0 && focusedIndex < Items.Count)
//                     await ToggleItem(ValueSelector(Items[focusedIndex]));
//                 break;
//             case "Escape":
//                 e.PreventDefault();
//                 isOpen = false;
//                 break;
//         }
//     }
//
//     private async Task ToggleItem(string value)
//     {
//         var newValues = new List<TValue>(CurrentValue ?? new());
//         bool added;
//         if (newValues.Contains((TValue)(object)value))
//         {
//             newValues.Remove((TValue)(object)value);
//             added = false;
//         }
//         else
//         {
//             newValues.Add((TValue)(object)value);
//             added = true;
//         }
//
//         CurrentValue = newValues;
//         await ValueChanged.InvokeAsync(CurrentValue);
//
//         AriaMessage = $"{value} {(added ? "selected" : "deselected")}";
//         StateHasChanged();
//     }
//
//     private void SelectAll()
//     {
//         CurrentValue = Items.Select(i => (TValue)(object)ValueSelector(i)).ToList();
//         AriaMessage = "All items selected";
//         StateHasChanged();
//     }
//
//     private void DeselectAll()
//     {
//         CurrentValue = new List<TValue>();
//         AriaMessage = "All items deselected";
//         StateHasChanged();
//     }
//
//     protected override bool TryParseValueFromString(string? value, out List<TValue> result, out string? validationErrorMessage)
//     {
//         result = new();
//         validationErrorMessage = null;
//         return true;
//     }
// }
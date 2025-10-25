using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorMultiSelect.Components.Pages;

public sealed partial class RkFileUpload : IAsyncDisposable
{
    [Inject] public required IJSRuntime JsRuntime { get; set; }

    [Parameter, EditorRequired] public string Id { get; set; } = string.Empty;
    [Parameter, EditorRequired] public string Label { get; set; } = string.Empty;
    [Parameter] public string Accept { get; set; } = ".docx, .pdf, .png, .jpg, .jpeg";
    [Parameter] public string CssClass { get; set; } = string.Empty;
    [Parameter, EditorRequired] public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }

    private bool isDragging;
    private IBrowserFile? _selectedFile;

    private IJSObjectReference? _jsModule;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/Pages/RkFileUpload.razor.js");
            await _jsModule.InvokeVoidAsync("init", Id);
        }
    }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        _selectedFile = e.File;
        await OnChange.InvokeAsync(e);
    }

    private async Task RemoveFile()
    {
        _selectedFile = null;
        await OnChange.InvokeAsync(new InputFileChangeEventArgs([]));
    }

    private void OnDragOver(DragEventArgs e)
    {
        isDragging = true;
    }

    private void OnDragLeave(DragEventArgs e)
    {
        isDragging = false;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule != null)
            {
                await _jsModule.InvokeVoidAsync("dispose", Id);
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        { }
    }
}

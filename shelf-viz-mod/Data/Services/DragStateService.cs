using Microsoft.JSInterop;
public class DragStateService : IDragStateService
{
    private readonly IJSRuntime? _jsRuntime;
    public bool IsDragging { get; private set; }
    public event Action OnDragStateChanged = delegate { };

    public DragStateService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public void StartDragging()
    {
        IsDragging = true;
        LogToConsole("Start dragging");
        OnDragStateChanged?.Invoke();
    }

    public void EndDragging()
    {
        IsDragging = false;
        LogToConsole("End dragging");
        OnDragStateChanged?.Invoke();
    }

    private async void LogToConsole(string message)
    {
        await _jsRuntime.InvokeVoidAsync("console.log", message);
    }
}

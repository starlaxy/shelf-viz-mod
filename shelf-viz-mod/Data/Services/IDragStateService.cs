public interface IDragStateService
{
    bool IsDragging { get; }
    event Action OnDragStateChanged;

    void StartDragging();
    void EndDragging();
}

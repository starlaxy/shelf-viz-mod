using System.Reactive.Linq;
using System.Reactive.Subjects;
using shelf_viz_mod.Data.Services;

public class ModalService : IModalService
{
    private readonly BehaviorSubject<SKU?> _currentSKU = new BehaviorSubject<SKU?>(null);

    public IObservable<SKU?> CurrentSKU => _currentSKU.AsObservable();

    public void OpenModal(SKU sku)
    {
        _currentSKU.OnNext(sku);
    }

    public void CloseModal()
    {
        _currentSKU.OnNext(null);
    }
}

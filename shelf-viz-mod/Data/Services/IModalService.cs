
namespace shelf_viz_mod.Data.Services
{
    public interface IModalService
    {
        IObservable<SKU?> CurrentSKU { get; }
        void OpenModal(SKU sku);
        void CloseModal();


    }
}
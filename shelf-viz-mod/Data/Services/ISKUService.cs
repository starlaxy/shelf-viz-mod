namespace shelf_viz_mod.Data.Services
{
    public interface ISKUService
    {
        Task InitializeAsync();  // Updated method name
        IObservable<IEnumerable<SKU>> GetAllSKUsAsync();
        IObservable<SKU?> GetSKUByJanCodeAsync(string janCode);
    }
}

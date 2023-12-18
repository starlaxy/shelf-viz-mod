using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace shelf_viz_mod.Data.Services
{
    public interface ISKUService
    {
        Task InitializeFromCsvAsync(string filePath);
        IObservable<IEnumerable<SKU>> GetAllSKUsAsync();
        IObservable<SKU?> GetSKUByJanCodeAsync(string janCode);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using shelf_viz_mod.Data.Models;  // Ensure this matches your project's structure

namespace shelf_viz_mod.Data.Services
{
    public interface ISKUService
    {
        Task<IEnumerable<SKU>> GetAllSKUsAsync();
        Task<SKU> GetSKUByIdAsync(string janCode);
        Task AddSKUAsync(SKU newSKU);
        Task UpdateSKUAsync(SKU updatedSKU);
        Task DeleteSKUAsync(string janCode);
    }
}

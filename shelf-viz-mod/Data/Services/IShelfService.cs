using System.Collections.Generic;
using System.Threading.Tasks;
using shelf_viz_mod.Data.Models;  // Adjust the namespace based on your project

namespace shelf_viz_mod.Data.Services
{
    public interface IShelfService
    {
        Task<IEnumerable<Shelf>> GetShelvesAsync();
        // You can add more methods as per your requirements
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shelf_viz_mod.Data.Models;  // Ensure this matches your project's structure

namespace shelf_viz_mod.Data.Services
{
    public class SKUService : ISKUService
    {
        private readonly List<SKU> _skus;

        public SKUService()
        {
            _skus = new List<SKU>
            {
                // Initialize with some mock data
                new SKU { JanCode = "12345", Name = "Beverage 1", /* other properties */ },
                new SKU { JanCode = "67890", Name = "Beverage 2", /* other properties */ }
            };
        }

        public async Task<IEnumerable<SKU>> GetAllSKUsAsync()
        {
            // In a real application, replace this with actual data access code
            return await Task.FromResult(_skus);
        }

        public async Task<SKU> GetSKUByIdAsync(string janCode)
        {
            var sku = _skus.FirstOrDefault(s => s.JanCode == janCode);
            return await Task.FromResult(sku);
        }

        public async Task AddSKUAsync(SKU newSKU)
        {
            if (newSKU == null)
                throw new ArgumentNullException(nameof(newSKU));

            if (!_skus.Any(s => s.JanCode == newSKU.JanCode))
            {
                _skus.Add(newSKU);
            }
            else
            {
                throw new InvalidOperationException($"SKU with JanCode {newSKU.JanCode} already exists.");
            }

            await Task.CompletedTask;
        }

        public async Task UpdateSKUAsync(SKU updatedSKU)
        {
            if (updatedSKU == null)
                throw new ArgumentNullException(nameof(updatedSKU));

            var skuIndex = _skus.FindIndex(s => s.JanCode == updatedSKU.JanCode);
            if (skuIndex != -1)
            {
                _skus[skuIndex] = updatedSKU;
            }
            else
            {
                throw new KeyNotFoundException($"SKU with JanCode {updatedSKU.JanCode} not found.");
            }

            await Task.CompletedTask;
        }

        public async Task DeleteSKUAsync(string janCode)
        {
            var sku = _skus.FirstOrDefault(s => s.JanCode == janCode);
            if (sku != null)
            {
                _skus.Remove(sku);
            }
            else
            {
                throw new KeyNotFoundException($"SKU with JanCode {janCode} not found.");
            }

            await Task.CompletedTask;
        }
    }
}

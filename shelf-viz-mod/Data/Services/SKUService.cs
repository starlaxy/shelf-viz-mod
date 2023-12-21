using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CsvHelper;
using System.Globalization;
using System.IO;
using shelf_viz_mod.Data.Models;
using shelf_viz_mod.Data.Services;

public class SKUService : ISKUService
{
    private readonly ILocalStorageService _localStorage;
    private readonly BehaviorSubject<IEnumerable<SKU>> _skusSubject;
    private readonly HttpClient _httpClient;

    public SKUService(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
        _skusSubject = new BehaviorSubject<IEnumerable<SKU>>(new List<SKU>());
    }

    public async Task InitializeAsync()
    {
        var skusFromStorage = await _localStorage.GetItemAsync<List<SKU>>("skus");
        if (skusFromStorage == null || !skusFromStorage.Any())
        {
            await LoadDataFromCsvAsync(); // Load from CSV and update local storage
        }
        else
        {
            _skusSubject.OnNext(skusFromStorage); // Use data from local storage
        }
    }

    public SKU CreateSKU(string janCode, string name, float x, float y, float z, string imageUrl, int size, SKUShape shape, string? timeStampString = null)
    {
        long timeStamp;

        if (string.IsNullOrEmpty(timeStampString))
        {
            // Get current Unix time in seconds
            timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        else
        {
            if (!long.TryParse(timeStampString, out timeStamp))
            {
                throw new ArgumentException("Invalid timestamp value.");
            }

        }

        return new SKU
        {
            JanCode = janCode,
            Name = name,
            X = x,
            Y = y,
            Z = z,
            ImageURL = imageUrl,
            Size = size,
            TimeStamp = timeStamp,
            Shape = shape
        };
    }

    private async Task LoadDataFromCsvAsync()
    {
        var csvContent = await _httpClient.GetStringAsync("sample-data/sku.csv");
        using var reader = new StringReader(csvContent);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var skusList = csv.GetRecords<SKU>().ToList();

        _skusSubject.OnNext(skusList);

        await _localStorage.SetItemAsync("skus", skusList); // Save to local storage
    }

    public IObservable<IEnumerable<SKU>> GetAllSKUsAsync()
    {
        return _skusSubject.AsObservable();
    }

    public IObservable<SKU?> GetSKUByJanCodeAsync(string janCode)
    {
        return _skusSubject.AsObservable()
            .SelectMany(skus => skus)
            .Where(sku => sku.JanCode == janCode)
            .FirstOrDefaultAsync();
    }
}

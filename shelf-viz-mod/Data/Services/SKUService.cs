using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Reactive.Subjects;
using Blazored.LocalStorage;
using shelf_viz_mod.Data.Services;

public class SKUService : ISKUService
{
    private readonly ILocalStorageService _localStorage;
    private readonly BehaviorSubject<IEnumerable<SKU>> _skusSubject;
    private IEnumerable<SKU>? _skus;


    public SKUService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _skusSubject = new BehaviorSubject<IEnumerable<SKU>>(new List<SKU>());
    }

    public async Task InitializeFromCsvAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var skusList = new List<SKU>();
        await foreach (var record in csv.GetRecordsAsync<SKU>())
        {
            skusList.Add(record);
        }

        _skus = skusList;
        _skusSubject.OnNext(_skus);

        // Optionally store to local storage for persistence across sessions
        await _localStorage.SetItemAsync("skus", _skus);
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

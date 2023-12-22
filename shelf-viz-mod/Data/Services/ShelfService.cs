using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using shelf_viz_mod.Data.Models;
using Microsoft.Extensions.Logging;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Linq;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

[assembly: InternalsVisibleTo("shelf-viz-mod.Tests")]
namespace shelf_viz_mod.Data.Services
{
    public class ShelfService : IShelfService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ShelfService> _logger;
        private readonly IScopedServiceFactory _scopedServiceFactory;
        private BehaviorSubject<IEnumerable<Cabinet>> _cabinetSubject;
        public event Action CabinetsUpdated = delegate { };
        private readonly IWebAssemblyHostEnvironment _environment;

        public ShelfService(IHttpClientFactory httpClientFactory,
                        ILogger<ShelfService> logger,
                        IScopedServiceFactory scopedServiceFactory,
                        IWebAssemblyHostEnvironment environment) // Injecting IWebAssemblyHostEnvironment
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopedServiceFactory = scopedServiceFactory ?? throw new ArgumentNullException(nameof(scopedServiceFactory));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment)); // Storing the injected environment
            _cabinetSubject = new BehaviorSubject<IEnumerable<Cabinet>>(new List<Cabinet>());
        }
        public async Task InitializeAsync()
        {
            await InitializeData();
        }
        private async Task LoadDataAsync()
        {
            try
            {
                // Determine the base address based on the environment
                string baseAddress = _environment.IsDevelopment()
                    ? "http://localhost:5290/"
                    : "https://gorgeous-stardust-085a96.netlify.app/";

                var requestUrl = $"{baseAddress}sample-data/shelf.json";
                _logger.LogInformation($"Request URL: {requestUrl}");

                var httpClient = _httpClientFactory.CreateClient();
                var jsonData = await httpClient.GetStringAsync(requestUrl);


                // var httpClient = _httpClientFactory.CreateClient();

                // var baseAddress = _httpClientFactory.CreateClient().BaseAddress?.ToString() ?? string.Empty;
                // var requestUrl = $"{baseAddress}sample-data/shelf.json";

                Console.WriteLine($"Request URL: {httpClient.BaseAddress}{requestUrl}");


                var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
                // var jsonData = await httpClient.GetStringAsync("sample-data/shelf.json");
                // var jsonData = await _httpClientFactory.CreateClient().GetStringAsync(requestUrl);

                var shelfData = JsonSerializer.Deserialize<ShelfData>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (shelfData?.Cabinets != null)
                {
                    await localStorage.SetItemAsync("shelfLayout", shelfData.Cabinets);
                    _cabinetSubject.OnNext(shelfData.Cabinets);
                }
                else
                {
                    _logger.LogWarning("No cabinets data found in the JSON file.");
                    _cabinetSubject.OnNext(new List<Cabinet>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading data from shelf.json");
                throw;
            }
        }

        private async Task InitializeData()
        {
            var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
            var cabinets = await localStorage.GetItemAsync<List<Cabinet>>("shelfLayout");
            if (cabinets == null || !cabinets.Any())
            {
                await LoadDataAsync();
            }
            else
            {
                _cabinetSubject.OnNext(cabinets);
            }
        }

        // Remaining CRUD methods using _scopedServiceFactory to access ILocalStorageService

        public async Task SwapLanesAsync(int sourceCabinetId, int sourceRowId, int sourceLaneId, int targetCabinetId, int targetRowId, int targetLaneId)
        {
            _logger.LogInformation("Starting SwapLanesAsync");
            var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
            var cabinets = await localStorage.GetItemAsync<List<Cabinet>>("shelfLayout") ?? new List<Cabinet>();

            // Find the source and target lanes
            var sourceLane = FindLane(cabinets, sourceCabinetId, sourceRowId, sourceLaneId);
            var targetLane = FindLane(cabinets, targetCabinetId, targetRowId, targetLaneId);

            if (sourceLane != null && targetLane != null)
            {
                // Swap the lane data
                (sourceLane.JanCode, targetLane.JanCode) = (targetLane.JanCode, sourceLane.JanCode);
                (sourceLane.Quantity, targetLane.Quantity) = (targetLane.Quantity, sourceLane.Quantity);

                await localStorage.SetItemAsync("shelfLayout", cabinets);
                _logger.LogInformation("Emitting updated cabinets");
                _cabinetSubject.OnNext(cabinets);
            }
            else
            {
                _logger.LogError("Failed to find lanes for swapping.");
            }
            _logger.LogInformation("SwapLanesAsync completed successfully");

            CabinetsUpdated?.Invoke();
        }
        public IObservable<IEnumerable<Cabinet?>> GetAllCabinetsAsync()
        {
            return _cabinetSubject.AsObservable();
        }

        public IObservable<Cabinet?> GetCabinetByIdAsync(int cabinetId)
        {
            return GetAllCabinetsAsync()
                   .SelectMany(cabinets => cabinets)
                   .FirstOrDefaultAsync(cabinet => cabinet?.Number == cabinetId);
        }

        public IObservable<IEnumerable<Row?>> GetRowsInCabinetAsync(int cabinetId)
        {
            return GetCabinetByIdAsync(cabinetId)
                   .Select(cabinet => cabinet?.Rows ?? Enumerable.Empty<Row>());
        }

        public IObservable<Row?> GetRowByIdAsync(int cabinetId, int rowId)
        {
            return GetRowsInCabinetAsync(cabinetId)
                   .SelectMany(rows => rows)
                   .FirstOrDefaultAsync(row => row?.Number == rowId);
        }

        public IObservable<IEnumerable<Lane?>> GetLanesInRowAsync(int cabinetId, int rowId)
        {
            return GetRowByIdAsync(cabinetId, rowId)
                   .Select(row => row?.Lanes ?? Enumerable.Empty<Lane>());
        }

        public IObservable<Lane?> GetLaneByIdAsync(int cabinetId, int rowId, int laneId)
        {
            return GetLanesInRowAsync(cabinetId, rowId)
                   .SelectMany(lanes => lanes)
                   .FirstOrDefaultAsync(lane => lane?.Number == laneId);
        }
        private Lane? FindLane(List<Cabinet> cabinets, int cabinetId, int rowId, int laneId)
        {
            return cabinets.FirstOrDefault(c => c.Number == cabinetId)
                           ?.Rows.FirstOrDefault(r => r.Number == rowId)
                           ?.Lanes.FirstOrDefault(l => l.Number == laneId);
        }


        public async Task DeleteCabinetAsync(int cabinetId)
        {
            var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
            var cabinets = await localStorage.GetItemAsync<List<Cabinet>>("shelfLayout");
            cabinets.RemoveAll(c => c.Number == cabinetId);
            await localStorage.SetItemAsync("shelfLayout", cabinets);
            _cabinetSubject.OnNext(cabinets);
        }

        public async Task UpdateRowAsync(int cabinetId, Row updatedRow)
        {
            var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
            var cabinets = await localStorage.GetItemAsync<List<Cabinet>>("shelfLayout") ?? new List<Cabinet>();
            var cabinet = cabinets.FirstOrDefault(c => c.Number == cabinetId);
            if (cabinet != null)
            {
                var rowIndex = cabinet.Rows.FindIndex(r => r.Number == updatedRow.Number);
                if (rowIndex != -1)
                {
                    cabinet.Rows[rowIndex] = updatedRow;
                    await localStorage.SetItemAsync("shelfLayout", cabinets);
                    _cabinetSubject.OnNext(cabinets);
                }
            }
        }

        public async Task DeleteRowAsync(int cabinetId, int rowId)
        {
            var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
            var cabinets = await localStorage.GetItemAsync<List<Cabinet>>("shelfLayout");
            var cabinet = cabinets.FirstOrDefault(c => c.Number == cabinetId);
            cabinet?.Rows.RemoveAll(r => r.Number == rowId);
            await localStorage.SetItemAsync("shelfLayout", cabinets);
            _cabinetSubject.OnNext(cabinets);
        }

        public async Task DeleteLaneAsync(int cabinetId, int rowId, int laneId)
        {
            var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
            var cabinets = await localStorage.GetItemAsync<List<Cabinet>>("shelfLayout");
            var cabinet = cabinets.FirstOrDefault(c => c.Number == cabinetId);
            var row = cabinet?.Rows.FirstOrDefault(r => r.Number == rowId);
            row?.Lanes.RemoveAll(l => l.Number == laneId);
            await localStorage.SetItemAsync("shelfLayout", cabinets);
            _cabinetSubject.OnNext(cabinets);
        }

        public async Task UpdateCabinetAsync(Cabinet updatedCabinet)
        {
            var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
            var cabinets = await localStorage.GetItemAsync<List<Cabinet>>("shelfLayout") ?? new List<Cabinet>();
            var index = cabinets.FindIndex(c => c.Number == updatedCabinet.Number);
            if (index != -1)
            {
                cabinets[index] = updatedCabinet;
                await localStorage.SetItemAsync("shelfLayout", cabinets);
                _cabinetSubject.OnNext(cabinets);
            }
            else
            {
                throw new KeyNotFoundException($"Cabinet with ID {updatedCabinet.Number} not found.");
            }
        }

        public async Task UpdateLaneAsync(int cabinetId, int rowId, Lane updatedLane)
        {
            var localStorage = _scopedServiceFactory.GetScopedService<ILocalStorageService>();
            var cabinets = await localStorage.GetItemAsync<List<Cabinet>>("shelfLayout") ?? new List<Cabinet>();
            var cabinet = cabinets.FirstOrDefault(c => c.Number == cabinetId);
            if (cabinet == null)
            {
                throw new KeyNotFoundException($"Cabinet with ID {cabinetId} not found.");
            }

            var row = cabinet.Rows.FirstOrDefault(r => r.Number == rowId);
            if (row == null)
            {
                throw new KeyNotFoundException($"Row with ID {rowId} in Cabinet {cabinetId} not found.");
            }

            var laneIndex = row.Lanes.FindIndex(l => l.Number == updatedLane.Number);
            if (laneIndex != -1)
            {
                row.Lanes[laneIndex] = updatedLane;
                await localStorage.SetItemAsync("shelfLayout", cabinets);
                _cabinetSubject.OnNext(cabinets);
            }
            else
            {
                throw new KeyNotFoundException($"Lane with ID {updatedLane.Number} in Row {rowId} not found.");
            }
        }


    }

}

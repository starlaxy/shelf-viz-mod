using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using shelf_viz_mod.Data.Models;
using Microsoft.Extensions.Logging;

namespace shelf_viz_mod.Data.Services
{
    public class ShelfService : IShelfService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShelfService> _logger;
        private BehaviorSubject<IEnumerable<Cabinet>> _cabinetSubject;

        private ShelfService(ILocalStorageService localStorage, HttpClient httpClient, ILogger<ShelfService> logger)
        {
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cabinetSubject = new BehaviorSubject<IEnumerable<Cabinet>>(new List<Cabinet>()); // Initialized with an empty list
        }

        public static async Task<ShelfService> CreateAsync(ILocalStorageService localStorage, HttpClient httpClient, ILogger<ShelfService> logger)
        {
            var service = new ShelfService(localStorage, httpClient, logger);
            await service.LoadDataAsync();
            return service;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var jsonData = await _httpClient.GetStringAsync("sample-data/shelf.json");
                var cabinets = JsonSerializer.Deserialize<List<Cabinet>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (cabinets != null)
                {
                    await _localStorage.SetItemAsync("shelfLayout", cabinets);
                    _cabinetSubject = new BehaviorSubject<IEnumerable<Cabinet>>(cabinets);
                }
                else
                {
                    _logger.LogWarning("No cabinets data found in the JSON file.");
                    _cabinetSubject = new BehaviorSubject<IEnumerable<Cabinet>>(new List<Cabinet>());
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
            try
            {
                var cabinets = await _localStorage.GetItemAsync<List<Cabinet>>("shelfLayout") ?? new List<Cabinet>();
                _cabinetSubject = new BehaviorSubject<IEnumerable<Cabinet>>(cabinets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize data in ShelfService");
                // Reset _cabinetSubject with an empty list 
                _cabinetSubject = new BehaviorSubject<IEnumerable<Cabinet>>(new List<Cabinet>());
            }
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
        public async Task DeleteCabinetAsync(int cabinetId)
        {
            var cabinets = await _localStorage.GetItemAsync<List<Cabinet>>("shelfLayout");
            cabinets.RemoveAll(c => c.Number == cabinetId);
            await _localStorage.SetItemAsync("shelfLayout", cabinets);
            _cabinetSubject.OnNext(cabinets);
        }

        public async Task UpdateRowAsync(int cabinetId, Row updatedRow)
        {
            var cabinets = await _localStorage.GetItemAsync<List<Cabinet>>("shelfLayout") ?? new List<Cabinet>();
            var cabinet = cabinets.FirstOrDefault(c => c.Number == cabinetId);
            if (cabinet != null)
            {
                var rowIndex = cabinet.Rows.FindIndex(r => r.Number == updatedRow.Number);
                if (rowIndex != -1)
                {
                    cabinet.Rows[rowIndex] = updatedRow;
                    await _localStorage.SetItemAsync("shelfLayout", cabinets);
                    _cabinetSubject.OnNext(cabinets);
                }
            }
        }


        public async Task DeleteRowAsync(int cabinetId, int rowId)
        {
            var cabinets = await _localStorage.GetItemAsync<List<Cabinet>>("shelfLayout");
            var cabinet = cabinets.FirstOrDefault(c => c.Number == cabinetId);
            cabinet?.Rows.RemoveAll(r => r.Number == rowId);
            await _localStorage.SetItemAsync("shelfLayout", cabinets);
            _cabinetSubject.OnNext(cabinets);
        }

        public async Task DeleteLaneAsync(int cabinetId, int rowId, int laneId)
        {
            var cabinets = await _localStorage.GetItemAsync<List<Cabinet>>("shelfLayout");
            var cabinet = cabinets.FirstOrDefault(c => c.Number == cabinetId);
            var row = cabinet?.Rows.FirstOrDefault(r => r.Number == rowId);
            row?.Lanes.RemoveAll(l => l.Number == laneId);
            await _localStorage.SetItemAsync("shelfLayout", cabinets);
            _cabinetSubject.OnNext(cabinets);
        }
        public async Task UpdateCabinetAsync(Cabinet updatedCabinet)
        {
            var cabinets = await _localStorage.GetItemAsync<List<Cabinet>>("shelfLayout") ?? new List<Cabinet>();
            var index = cabinets.FindIndex(c => c.Number == updatedCabinet.Number);
            if (index != -1)
            {
                cabinets[index] = updatedCabinet;
                await _localStorage.SetItemAsync("shelfLayout", cabinets);
                _cabinetSubject.OnNext(cabinets);
            }
            else
            {
                throw new KeyNotFoundException($"Cabinet with ID {updatedCabinet.Number} not found.");
            }
        }
        public async Task UpdateLaneAsync(int cabinetId, int rowId, Lane updatedLane)
        {
            var cabinets = await _localStorage.GetItemAsync<List<Cabinet>>("shelfLayout") ?? new List<Cabinet>();
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
                await _localStorage.SetItemAsync("shelfLayout", cabinets);
                _cabinetSubject.OnNext(cabinets);
            }
            else
            {
                throw new KeyNotFoundException($"Lane with ID {updatedLane.Number} in Row {rowId} not found.");
            }
        }


    }
}

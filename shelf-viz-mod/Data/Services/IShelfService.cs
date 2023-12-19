using shelf_viz_mod.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Blazored.LocalStorage;
using System;

namespace shelf_viz_mod.Data.Services
{
    public interface IShelfService
    {
        IObservable<IEnumerable<Cabinet?>> GetAllCabinetsAsync();
        IObservable<Cabinet?> GetCabinetByIdAsync(int cabinetId);
        IObservable<IEnumerable<Row?>> GetRowsInCabinetAsync(int cabinetId);
        IObservable<Row?> GetRowByIdAsync(int cabinetId, int rowId);
        IObservable<IEnumerable<Lane?>> GetLanesInRowAsync(int cabinetId, int rowId);
        IObservable<Lane?> GetLaneByIdAsync(int cabinetId, int rowId, int laneId);
        Task UpdateCabinetAsync(Cabinet cabinet);
        Task UpdateRowAsync(int cabinetId, Row row);
        Task UpdateLaneAsync(int cabinetId, int rowId, Lane lane);
        Task DeleteCabinetAsync(int cabinetId);
        Task DeleteRowAsync(int cabinetId, int rowId);
        Task DeleteLaneAsync(int cabinetId, int rowId, int laneId);
        Task InitializeAsync();
        Task SwapLanesAsync(int sourceCabinetId, int sourceRowId, int sourceLaneId,
                            int targetCabinetId, int targetRowId, int targetLaneId);
        event Action CabinetsUpdated;

    }
}

namespace Carseer.Api.Interfaces
{
    public interface IVehicleService
    {
        Task<List<string>> GetModelsForMakeIdYear(int makeId, int modelyear);
    }
}

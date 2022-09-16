using Ipe.Domain.Models;
using Ipe.UseCases.PlantUseCase.GetActiveTreeBiomes;

namespace Ipe.UseCases.Interfaces.Repositories;
public interface IPlantRepository : IBaseRepository<Plant>
{
    Task<Plant> FindSomePlantByUserId(string UserId);
    Task<Plant> GetPlantById(string PlantId);
    Task<PlantAndTotal> GetPlantByFilter(GetActivePlantUseCaseInput filter);
    Task RecoveryPlants(string NewUserId, string UserEmail);
}
using Ipe.Domain.Models;

namespace Ipe.UseCases.PlantUseCase.GetActiveTreeBiomes;
public class GetActivePlantUseCaseOutput
{
    public IEnumerable<Plant>? Plants { get; set; }
    public long? TotalCount { get; set; }
}

using Ipe.UseCases.Interfaces.Repositories;

namespace Ipe.UseCases.PlantUseCase.GetActiveTreeBiomes;
public class GetActivePlantUseCase : IUseCase<GetActivePlantUseCaseInput, GetActivePlantUseCaseOutput>
{
    private IPlantRepository _plantRepository { get; }

    public GetActivePlantUseCase(IPlantRepository plantRepository)
    {
        _plantRepository = plantRepository;
    }

    public async Task<GetActivePlantUseCaseOutput> Run(GetActivePlantUseCaseInput input)
    {
        return await _plantRepository
            .GetPlantByFilter(input)
            .ContinueWith(plantsAndTotal => new GetActivePlantUseCaseOutput
            {
                Plants = plantsAndTotal.Result.Plants,
                TotalCount = plantsAndTotal.Result.TotalCount
            });
    }
}

using Ipe.Domain.Errors;
using Ipe.Domain.Models;
using Ipe.UseCases.Interfaces.Repositories;

namespace Ipe.UseCases.PlantCustomizeUseCase;
public class PlantCustomizeUseCase : IUseCase<PlantCustomizeUseCaseInput, PlantCustomizeUseCaseOutput>
{
    private readonly IPlantRepository _plantRepository;

    public PlantCustomizeUseCase(
        IPlantRepository plantRepository
    )
    {
        _plantRepository = plantRepository;
    }

    public async Task<PlantCustomizeUseCaseOutput> Run(PlantCustomizeUseCaseInput Input)
    {
        var Plant = await GetPlantById(Input);

        ValidatePlantCustomize(Input, Plant);
        await PerformCustomize(Input, Plant);

        return new PlantCustomizeUseCaseOutput();
    }

    private async Task PerformCustomize(PlantCustomizeUseCaseInput Input, Plant Plant)
    {
        Plant.Name = Input.TreeName;
        Plant.Message = Input.TreeMessage;
        Plant.Hastags = Input.TreeHastags;

        await _plantRepository.Update(Plant);
    }

    private static void ValidatePlantCustomize(PlantCustomizeUseCaseInput Input, Plant Plant)
    {
        if (Plant is null)
            throw new InvalidPlantIdException();

        if (ValidatePlantingUser(Input, Plant))
            throw new PlantCustomizeException();

        if (ValidateCustomizePerformed(Plant))
            throw new PlantCustomizePerformedException();
    }

    private static bool ValidateCustomizePerformed(Plant Plant)
    {
        return !string.IsNullOrEmpty(Plant.Name) ||
            !string.IsNullOrEmpty(Plant.Message) ||
            (Plant.Hastags is not null && Plant.Hastags.Any());
    }

    private static bool ValidatePlantingUser(PlantCustomizeUseCaseInput Input, Plant Plant)
    {
        return !Input.UserId.Equals(Plant.UserId);
    }

    private async Task<Plant> GetPlantById(PlantCustomizeUseCaseInput Input)
    {
        return await _plantRepository.GetPlantById(Input.PlantId);
    }
}

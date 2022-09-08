using Ipe.Domain.Errors;
using Ipe.Domain.Models;
using Ipe.UseCases.Interfaces.Repositories;
using Ipe.UseCases.PlantUseCase.GetPlantDetailUseCase;

namespace Ipe.UseCases.PlantUseCase.GetPlantDetailUseCase;
public class GetPlantDetailUseCase : IUseCase<GetPlantDetailUseCaseInput, GetPlantDetailUseCaseOutput>
{
    private readonly IPlantRepository _plantRepository;

    public GetPlantDetailUseCase(
        IPlantRepository plantRepository
    )
    {
        _plantRepository = plantRepository;
    }

    public async Task<GetPlantDetailUseCaseOutput> Run(GetPlantDetailUseCaseInput Input)
    {
        var Plant = await _plantRepository.GetPlantById(Input.PlantId);

        if (Plant is null)
        {
            throw new InvalidPlantIdException();
        }

        if (Plant.UserId != Input.UserId)
        {
            throw new UnauthorizedException();
        }


        return BuildOutput(Plant);
    }

    private static bool CanEditPlant(Plant Plant)
    {
        return (Plant.Name is null && Plant.Message is null && Plant.Hastags.Count == 0);
    }

    private static GetPlantDetailUseCaseOutput BuildOutput(Plant Plant)
    {
        return new GetPlantDetailUseCaseOutput
        {
            PlantId = Plant.Id,
            OrderId = Plant.OrderId,
            TreeId = Plant.TreeId,
            Name = Plant.Name,
            Message = Plant.Message,
            Hastags = Plant.Hastags,
            Biome = Plant.Biome,
            Species = Plant.Species,
            Image = Plant.Image,
            Description = Plant.Description,
            Value = Plant.Value,
            CreatedAt = Plant.CreatedAt,
            UpdatedAt = Plant.UpdatedAt,
            CanEdit = CanEditPlant(Plant)
        };
    }
}

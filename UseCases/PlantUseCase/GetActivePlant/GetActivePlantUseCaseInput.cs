namespace Ipe.UseCases.PlantUseCase.GetActiveTreeBiomes;
public class GetActivePlantUseCaseInput
{
    public string UserId { get; set; }
    public string? Biome { get; set; }
    public string? Name { get; set; }
    public string? Species { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public bool? RequiredTotal { get; set; }
}
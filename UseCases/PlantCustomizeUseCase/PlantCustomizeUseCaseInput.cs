namespace Ipe.UseCases.PlantCustomizeUseCase;
public class PlantCustomizeUseCaseInput
{
    public string UserId { get; set; }
    public string PlantId { get; set; }
    public string TreeName { get; set; }
    public string TreeMessage { get; set; }
    public List<string> TreeHastags { get; set; }
}

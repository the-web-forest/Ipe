namespace Ipe.UseCases.TreeUseCase.GetTreesByFilter
{
    public class GetTreesByFilterUseCaseInput
    {
        public string? Biome { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? RequiredTotal { get; set; }
    }
}

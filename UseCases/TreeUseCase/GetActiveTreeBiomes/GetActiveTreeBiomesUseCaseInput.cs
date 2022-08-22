namespace Ipe.UseCases.TreeUseCase.GetActiveTreeBiomes
{
    public class GetActiveTreeBiomesUseCaseInput
    {
        public string? Name { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? RequiredTotal { get; set; }
    }
}

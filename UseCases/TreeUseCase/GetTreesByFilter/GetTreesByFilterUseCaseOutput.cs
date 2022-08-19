namespace Ipe.UseCases.TreeUseCase.GetTreesByFilter
{
    public class GetTreesByFilterUseCaseOutput
    {
        public IEnumerable<TreeByFilter> Trees { get; set; }
        public long? TotalCount { get; set; }
    }
}

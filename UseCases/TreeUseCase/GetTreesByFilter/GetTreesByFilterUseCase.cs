using Ipe.UseCases.Interfaces.Repositories;

namespace Ipe.UseCases.TreeUseCase.GetTreesByFilter
{
    public class GetTreesByFilterUseCase : IUseCase<GetTreesByFilterUseCaseInput, GetTreesByFilterUseCaseOutput>
    {
        private readonly ITreeRepository _treeRepository;

        public GetTreesByFilterUseCase(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<GetTreesByFilterUseCaseOutput> Run(GetTreesByFilterUseCaseInput input)
        {
            return await _treeRepository
                .GetTreesByFilter(input)
                .ContinueWith(treesAndTotal => new GetTreesByFilterUseCaseOutput
                {
                    Trees = treesAndTotal.Result.Trees.Select(tree => new TreeByFilter(
                        tree.Id,
                        tree.Name,
                        tree.Description,
                        tree.Image,
                        tree.Value,
                        tree.Biome
                    )),
                    TotalCount = treesAndTotal.Result.TotalCount
                });
        }
    }
}

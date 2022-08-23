using Ipe.UseCases.Interfaces.Repositories;

namespace Ipe.UseCases.TreeUseCase.GetActiveTreeBiomes
{
    public class GetActiveTreeBiomesUseCase : IUseCase<GetActiveTreeBiomesUseCaseInput, GetActiveTreeBiomesUseCaseOutput>
    {
        private readonly ITreeRepository _treeRepository;

        public GetActiveTreeBiomesUseCase(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<GetActiveTreeBiomesUseCaseOutput> Run(GetActiveTreeBiomesUseCaseInput input)
        {
            return await _treeRepository
                .GetActiveTreeBiomes(input)
                .ContinueWith(biomesAndTotal => new GetActiveTreeBiomesUseCaseOutput
                {
                    Biomes = biomesAndTotal.Result.Biomes,
                    TotalCount = biomesAndTotal.Result.TotalCount
                });
        }
    }
}

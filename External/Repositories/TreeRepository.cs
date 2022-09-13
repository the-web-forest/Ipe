using Ipe.Domain.Models;
using Ipe.UseCases.Interfaces.Repositories;
using Ipe.UseCases.TreeUseCase.GetActiveTreeBiomes;
using Ipe.UseCases.TreeUseCase.GetTreesByFilter;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ipe.External.Repositories
{
    public class TreeRepository : BaseRepository<Tree>, ITreeRepository
    {
        public TreeRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public async Task<List<Tree>> GetTreesById(List<string> TreeId)
        {
            return await _collection.Find(x => TreeId.Contains(x.Id)).ToListAsync();
        }

        public async Task<TreeAndTotal> GetTreesByFilter(GetTreesByFilterUseCaseInput filter)
        {
            var query = _collection
                .Aggregate()
                .Match(tree => tree.Deleted == false);
                
            if (!String.IsNullOrEmpty(filter.Biome))
                query = query
                    .Match(tree => !String.IsNullOrEmpty(tree.Biome) && tree.Biome.ToLower().Contains(filter.Biome.Trim().ToLower()));

            if (!String.IsNullOrEmpty(filter.Description))
                query = query
                    .Match(tree => !String.IsNullOrEmpty(tree.Description) && tree.Description.ToLower().Contains(filter.Description.Trim().ToLower()));

            if (!String.IsNullOrEmpty(filter.Name))
                query = query
                    .Match(tree => !String.IsNullOrEmpty(tree.Name) && tree.Name.ToLower().Contains(filter.Name.Trim().ToLower()));

            query = query
                .SortBy(tree => tree.Name);

            long? total = null;
            if (filter.RequiredTotal == true)
                total = query.Count().FirstOrDefault().Count;

            if (filter.Skip is not null)
                query = query
                    .Skip(filter.Skip ?? 0);

            if (filter.Take is not null)
                query = query
                    .Limit(filter.Take ?? 0);

            return await query
                .ToListAsync()
                .ContinueWith(trees => new TreeAndTotal
                {
                    Trees = trees.Result,
                    TotalCount = total
                });
        }

        public async Task<BiomeAndTotal> GetActiveTreeBiomes(GetActiveTreeBiomesUseCaseInput filter)
        {
            var query = _collection
                .Aggregate()
                .Match(tree => !tree.Deleted)
                .Group(tree => tree.Biome, trees => new { Biome = trees.Key });

            if (!String.IsNullOrEmpty(filter.Name))
                query = query
                    .Match(tree => !String.IsNullOrEmpty(tree.Biome) && tree.Biome.ToLower().Contains(filter.Name.Trim().ToLower()));

            query = query
                .SortBy(tree => tree.Biome);

            long? total = null;
            if (filter.RequiredTotal == true)
                total = query.Count().FirstOrDefault().Count;

            if (filter.Skip is not null)
                query = query
                    .Skip(filter.Skip ?? 0);

            if (filter.Take is not null)
                query = query
                    .Limit(filter.Take ?? 0);

            return await query
                .ToListAsync()
                .ContinueWith(biomes => new BiomeAndTotal
                {
                    Biomes = biomes.Result.Select(x => x.Biome),
                    TotalCount = total
                });
        }
    }
}

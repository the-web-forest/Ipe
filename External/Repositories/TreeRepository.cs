using Ipe.Domain.Models;
using Ipe.UseCases.Interfaces.Repositories;
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
                .Match(tree => tree.Deleted == false)
                .AppendStage<Tree>(new BsonDocument("$addFields", new BsonDocument("lowercaseName", new BsonDocument("$toLower", "$name"))));

            if (!String.IsNullOrEmpty(filter.Biome))
                query = query
                    .AppendStage<Tree>(new BsonDocument("$addFields", new BsonDocument("lowercaseBiome", new BsonDocument("$toLower", "$biome"))))
                    .Match(tree => !String.IsNullOrEmpty(tree.LowercaseBiome) && tree.LowercaseBiome.Contains(filter.Biome.Trim().ToLower()));

            if (!String.IsNullOrEmpty(filter.Description))
                query = query
                    .AppendStage<Tree>(new BsonDocument("$addFields", new BsonDocument("lowercaseDescription", new BsonDocument("$toLower", "$description"))))
                    .Match(tree => !String.IsNullOrEmpty(tree.LowercaseDescription) && tree.LowercaseDescription.Contains(filter.Description.Trim().ToLower()));

            if (!String.IsNullOrEmpty(filter.Name))
                query = query
                    .Match(tree => !String.IsNullOrEmpty(tree.LowercaseName) && tree.LowercaseName.Contains(filter.Name.Trim().ToLower()));

            query = query
                .AppendStage<Tree>(new BsonDocument("$sort", new BsonDocument { { "metadata.type", 1 }, { "lowercaseName", 1 } }));

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
    }
}

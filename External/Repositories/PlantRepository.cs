using Ipe.Domain.Models;
using Ipe.UseCases.Interfaces.Repositories;
using Ipe.UseCases.PlantUseCase.GetActiveTreeBiomes;
using MongoDB.Driver;
using System.Linq;

namespace Ipe.External.Repositories;
public class PlantRepository : BaseRepository<Plant>, IPlantRepository
{
    public PlantRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

    public async Task<Plant> FindSomePlantByUserId(string UserId)
    {
        return await _collection
            .Find(x => x.UserId == UserId)
            .FirstOrDefaultAsync();
    }

    public async Task<Plant> GetPlantById(string PlantId)
    {
        return await _collection
            .Find(x => x.Id == PlantId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Plant>> GetPlantByUserId(string UserId)
    {
        return await _collection
            .Find(plant => plant.UserId == UserId)
            .SortByDescending(plant => plant.CreatedAt)
            .ToListAsync();
    }

    public async Task<PlantAndTotal> GetPlantByFilter(GetActivePlantUseCaseInput filter)
    {
        var query = _collection
            .Aggregate()
            .Match(plant => plant.UserId.Equals(filter.UserId));

        query = ApplyFilterByName(filter.Name, query);
        query = ApplyFilterByBiome(filter.Biome, query);
        query = ApplyFilterBySpecies(filter.Species, query);

        long? total = GetTotal(filter.RequiredTotal, query);

        query = ApplySort(query);
        query = ApplySkip(filter.Skip, query);
        query = ApplyTake(filter.Take, query);

        return await query
            .ToListAsync()
            .ContinueWith(biomes => new PlantAndTotal
            {
                Plants = biomes.Result.Select(x => x),
                TotalCount = total
            });
    }

    private IAggregateFluent<Plant> ApplySort(IAggregateFluent<Plant> query)
    {
        query = query.SortByDescending(plant => plant.CreatedAt);
        return query;
    }

    private IAggregateFluent<Plant> ApplyTake(int? take, IAggregateFluent<Plant> query)
    {
        if (take is not null)
            query = query.Limit(take.Value);

        return query;
    }

    private IAggregateFluent<Plant> ApplySkip(int? skip, IAggregateFluent<Plant> query)
    {
        if (skip is not null)
            query = query.Skip(skip.Value);

        return query;
    }

    private long? GetTotal(bool? requiredTotal, IAggregateFluent<Plant> query)
    {
        return (requiredTotal == true)
            ? query.Count().FirstOrDefault().Count
            : null;
    }

    private IAggregateFluent<Plant> ApplyFilterBySpecies(string? species, IAggregateFluent<Plant> query)
    {
        if (!string.IsNullOrEmpty(species))
        {
            species = species.Trim().ToLower();

            query = query
                .Match(plant =>
                    !string.IsNullOrEmpty(plant.Species) &&
                    plant.Species.ToLower().Contains(species)
                );
        }

        return query;
    }

    private IAggregateFluent<Plant> ApplyFilterByBiome(string? biome, IAggregateFluent<Plant> query)
    {
        if (!string.IsNullOrEmpty(biome))
        {
            biome = biome.Trim().ToLower();

            query = query
                .Match(plant =>
                    !string.IsNullOrEmpty(plant.Biome) &&
                    plant.Biome.ToLower().Contains(biome)
                );
        }

        return query;
    }

    private IAggregateFluent<Plant> ApplyFilterByName(string? name, IAggregateFluent<Plant> query)
    {
        if (!string.IsNullOrEmpty(name))
        {
            name = name.Trim().ToLower();

            query = query
                .Match(plant =>
                    !string.IsNullOrEmpty(plant.Name) &&
                    plant.Name.ToLower().Contains(name)
                );
        }

        return query;
    }
}

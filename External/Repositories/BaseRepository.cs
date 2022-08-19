using MongoDB.Driver;
using Ipe.Domain.Models;
using Ipe.Helpers;

namespace Ipe.External.Repositories
{
	public abstract class BaseRepository<T> where T: Model
	{

        private readonly IMongoDatabase _mongoDatabase;
		protected readonly IMongoCollection<T> _collection;

		public BaseRepository(IMongoDatabase mongoDatabase)
		{
			_mongoDatabase = mongoDatabase;
			_collection = _mongoDatabase.GetCollection<T>(typeof(T).Name);
		}

		public async Task Create(T Data)
        {
			Data.CreatedAt = DateHelper.BrazilDateTimeNow();
			Data.UpdatedAt = DateHelper.BrazilDateTimeNow();
            await _collection.InsertOneAsync(Data);
        }

		public async Task CreateMany(List<T> Data)
		{
			for(int i = 0; i< Data.Count; i++)
            {
				Data[i].CreatedAt = DateHelper.BrazilDateTimeNow();
                Data[i].UpdatedAt = DateHelper.BrazilDateTimeNow();
            }
			await _collection.InsertManyAsync(Data);
		}

		public async Task Update(T Data)
        {
			Data.UpdatedAt = DateHelper.BrazilDateTimeNow();
            await _collection.ReplaceOneAsync(doc => doc.Id == Data.Id, Data);
        }
	}
}


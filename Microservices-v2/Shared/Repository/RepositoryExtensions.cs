using Industria4.DomainModel;
using System.Threading.Tasks;

namespace Industria4.Repository
{
    public static class RepositoryExtensions
    {
        public static async Task AddIfNotExistAsync<T>(this IRepository<T> repository, T item)
            where T : IEntity
        {
            if (await repository.ExistAsync(item.Id))
				await repository.RemoveAsync(item.Id);

			await repository.AddAsync(item);
		}

        public static async Task ReplaceAsync<T>(this IRepository<T> repository, T item)
            where T : IEntity
        {
            T old = await repository.GetAsync(item.Id);
            if (old == null)
            {
                await repository.AddAsync(item);
            }
            else
            {
                await repository.RemoveAsync(item.Id);
                await repository.AddAsync(old);
            }
        }
    }
}
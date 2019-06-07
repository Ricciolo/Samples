using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Industria4.DomainModel;
using Industria4.Repository;
using Microsoft.EntityFrameworkCore;

namespace Industria4.EntityFramework.Repository
{
    /// <summary>
    ///     Base repository based on <see cref="DbContext" /> which map entity to a data model
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDataModel"></typeparam>
    /// <typeparam name="TDbContext"></typeparam>
    public abstract class DbMultiMapRepository<TEntity, TDataModel, TDbContext> : MultiRepository<TEntity>
        where TEntity : class, IEntity
        where TDataModel : class, new()
		where TDbContext : DbContext
    {
        protected DbMultiMapRepository(TDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        ///     Gets the current <see cref="DbContext" />
        /// </summary>
        public TDbContext Context { get; }

        /// <summary>
        ///     Gets the <see cref="DbSet" /> for the type
        /// </summary>
        protected virtual DbSet<TDataModel> DbSet => Context.Set<TDataModel>();

        /// <summary>
        ///     Converts the data model to a new entity instance
        /// </summary>
        /// <param name="dataModel"></param>
        /// <remarks>This method is called while reading a new entity</remarks>
        /// <returns></returns>
        protected abstract TEntity ToDomainModel(TDataModel dataModel);

        /// <summary>
        ///     Populates the data model using entity info
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dataModel"></param>
        /// <remarks>This method is called while adding or updating an entity</remarks>
        protected abstract void ToDataModel(TEntity entity, TDataModel dataModel);

        protected override async Task OnAddAsync(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                var dataModel = new TDataModel();
                ToDataModel(entity, dataModel);
                DbSet.Add(dataModel);
            }

            await Context.SaveChangesAsync();
        }

        protected override async Task OnUpdateAsync(IEnumerable<TEntity> entities)
        {
            TEntity[] entitiesArray = entities as TEntity[] ?? entities.ToArray();
            TDataModel[] dataModels = (await LoadDataModelAsync(entities.Select(e => e.Id))).ToArray();

            if (entitiesArray.Length != dataModels.Length) throw new InvalidOperationException("Cannot find all data models");

            int count = dataModels.Length;
            for (int x = 0; x < count; x++)
            {
                ToDataModel(entitiesArray[x], dataModels[x]);
            }

            await Context.SaveChangesAsync();
        }

        protected override async Task OnRemoveAsync(IEnumerable<string> ids)
        {
            IEnumerable<TDataModel> dataModels = (await LoadDataModelAsync(ids)).ToArray();

            foreach (TDataModel dataModel in dataModels)
            {
                RecursiveDelete(dataModel);
            }

            await Context.SaveChangesAsync();
        }

        protected virtual void RecursiveDelete(object value)
        {
            RecursiveDelete(value, new HashSet<object>());
        }

        protected virtual void RecursiveDelete(object value, HashSet<object> processed)
        {
            if (value == null || processed.Contains(value)) return;

            // Avoid infinite loop
            processed.Add(value);

            if (value is IList collection)
            {
                // Delete all elements contained into the collection
                foreach (object child in collection.Cast<object>().ToArray())
                {
                    RecursiveDelete(child, processed);
                    collection.Remove(child);
                }
                return;
            }

            Context.Entry(value).State = EntityState.Deleted;

            // Get all properties contained by the entity which are EF entities or collections
            foreach (PropertyInfo property in value.GetType()
                .GetRuntimeProperties()
                .Where(p =>
                {
                    Type type = p.PropertyType;
                    // EF entities or collections
                    return (type.IsClass && Context.Model.FindEntityType(type) != null)
                        || (type.IsGenericType && typeof(IList).IsAssignableFrom(type));
                }))
            {
                object childValue = property.GetValue(value);
                RecursiveDelete(childValue, processed);
            }
        }

        protected virtual void Merge<TEntityItem, TDataModelItem>(
            IEnumerable<TEntityItem> entities,
            IList<TDataModelItem> dataModels,
            Func<TEntityItem, TDataModelItem, bool> equality,
            Action<TEntityItem, TDataModelItem> setDataModel)
            where TDataModelItem : new()
        {
            Merge(entities, dataModels, equality, setDataModel, m => new TDataModelItem());
        }

        protected virtual void Merge<TEntityItem, TDataModelItem>(
            IEnumerable<TEntityItem> entities,
            IList<TDataModelItem> dataModels,
            Func<TEntityItem, TDataModelItem, bool> equality,
            Action<TEntityItem, TDataModelItem> setDataModel,
            Func<TEntityItem, TDataModelItem> dataModelFactory)
        {
            if (equality == null) throw new ArgumentNullException(nameof(equality));
            if (setDataModel == null) throw new ArgumentNullException(nameof(setDataModel));
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (dataModels == null) throw new ArgumentNullException(nameof(dataModels));

            var maps = dataModels
                .Select(d => new { DataModel = d, Entity = entities.FirstOrDefault(e => equality(e, d)) })
                .Concat(
                    entities
                        .Where(e => !dataModels.Any(d => equality(e, d)))
                        .Select(e => new { DataModel = default(TDataModelItem), Entity = e })
                )
                .ToArray();

            // Delete old entities
            foreach (var map in maps.Where(m => EqualityComparer<TEntityItem>.Default.Equals(m.Entity, default(TEntityItem))))
            {
                dataModels.Remove(map.DataModel);
                RecursiveDelete(map.DataModel);
            }

            // Update entities
            foreach (var map in maps.Where(m => !EqualityComparer<TEntityItem>.Default.Equals(m.Entity, default(TEntityItem)) &&
                                                    m.DataModel != null))
            {
                setDataModel(map.Entity, map.DataModel);
            }

            // Add new entities
            foreach (var map in maps.Where(m => m.DataModel == null))
            {
                var dataModel = dataModelFactory(map.Entity);
                setDataModel(map.Entity, dataModel);
                dataModels.Add(dataModel);
            }
        }

        protected abstract Task<IEnumerable<TDataModel>> LoadDataModelAsync(IEnumerable<string> ids);

        protected override async Task<IEnumerable<TEntity>> OnGetAsync(IEnumerable<string> ids)
        {
            IEnumerable<TDataModel> dataModels = await LoadDataModelAsync(ids);
			return dataModels.Select(dataModel => dataModel != null ? ToDomainModel(dataModel) : default(TEntity)).ToArray();
        }


        protected override async Task<IEnumerable<bool>> OnExistAsync(IEnumerable<string> ids)
        {
            IEnumerable<TDataModel> dataModels = (await LoadDataModelAsync(ids)).ToArray();
			return dataModels.Select(dataModel => dataModel != null).ToArray();
        }
        protected byte[] HashValue(string value)
        {
            using (MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(Encoding.Unicode.GetBytes(value));
            }
        }

        bool IsNullable<T>(T obj)
        {
            if (obj == null) return true; // obvious
            Type type = typeof(T);
            if (!type.IsValueType) return true; // ref-type
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T>
            return false; // value-type
        }
    }
}
using DataAccess.Repositories.Interfaces;
using LinqToDB;

namespace DataAccess.Repositories
{
    /// <summary>
    /// Generic repository to manage the database operations
    /// </summary>
    /// <typeparam name="T">Entity class to work with</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDataConnection _connection;

        public Repository(AppDataConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Implementation of the <see cref="IRepository{T}.Add(T)"/>
        /// </summary>
        public async Task Add(T entity)
        {
            await _connection.InsertAsync(entity);
        }

        /// <summary>
        /// Implementation of the <see cref="IRepository{T}.Update(T)"/>
        /// </summary>
        public async Task Update(T entity)
        {
            await _connection.UpdateAsync(entity);
        }

        /// <summary>
        /// Implementation of the <see cref="IRepository{T}.Delete(T)"/>
        /// </summary>
        public async Task Delete(T entity)
        {
            await _connection.DeleteAsync(entity);
        }
    }
}

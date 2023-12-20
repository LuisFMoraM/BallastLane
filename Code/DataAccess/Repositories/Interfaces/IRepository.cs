namespace DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Define a generic repository methods to manage
    /// the database operations
    /// </summary>
    /// <typeparam name="T">Entity class to work with</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Adds an Entity to the database
        /// </summary>
        /// <param name="entity">Entity to work with</param>
        Task Add(T entity);

        /// <summary>
        /// Updates an Entity in the database
        /// </summary>
        /// <param name="entity">Entity to work with</param>
        Task Update(T entity);

        /// <summary>
        /// Deletes an Entity from the database
        /// </summary>
        /// <param name="entity">Entity to work with</param>
        Task Delete(T entity);
    }
}

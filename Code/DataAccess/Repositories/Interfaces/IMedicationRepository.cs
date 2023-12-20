using DataAccess.Entities;

namespace DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Manages Medication actions in the DB
    /// </summary>
    public interface IMedicationRepository
    {
        /// <summary>
        /// Gets all the medications from the system
        /// </summary>
        /// <returns>Medications info</returns>
        Task<List<Medication>> GetAll();

        /// <summary>
        /// Gets a medication searching by id
        /// </summary>
        /// <param name="id">Medication identifier</param>
        /// <returns>Medication info</returns>
        Task<Medication?> GetById(long id);

        /// <summary>
        /// Adds a Medication to the database
        /// </summary>
        /// <param name="entity">Medication info</param>
        Task Add(Medication entity);

        /// <summary>
        /// Updates a Medication in the database
        /// </summary>
        /// <param name="entity">Medication info</param>
        Task Update(Medication entity);

        /// <summary>
        /// Deletes a Medication from the database
        /// </summary>
        /// <param name="entity">Medication info</param>
        Task Delete(Medication entity);
    }
}

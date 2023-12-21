using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    /// <summary>
    /// Define behaviors related to Medications
    /// </summary>
    public interface IMedicationService
    {
        /// <summary>
        /// Gets all the medications from the system
        /// </summary>
        /// <returns>Medications info</returns>
        Task<List<Medication>> GetAll();

        /// <summary>
        /// Adds a Medication to the system
        /// </summary>
        /// <param name="entity">Medication info</param>
        Task Add(Medication entity);

        /// <summary>
        /// Updates a Medication in the system
        /// </summary>
        /// <param name="id">Medication identifier</param>
        /// <param name="entity">Medication info</param>
        Task Update(long id, Medication entity);

        /// <summary>
        /// Deletes a Medication from the system
        /// </summary>
        /// <param name="id">Medication identifier</param>
        Task Delete(long id);
    }
}

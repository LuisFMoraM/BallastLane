using DataAccess.Entities;
using DataAccess.Repositories.Interfaces;
using LinqToDB;

namespace DataAccess.Repositories
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly IRepository<Medication> _repository;
        private readonly AppDataConnection _connection;

        public MedicationRepository(
            IRepository<Medication> repository, 
            AppDataConnection connection)
        {
            _repository = repository;
            _connection = connection;
        }

        /// <summary>
        /// Implementation of the <see cref="IMedicationRepository.GetAll"/>
        /// </summary>
        public Task<List<Medication>> GetAll() =>
            _connection.Medications.ToListAsync();

        /// <summary>
        /// Implementation of the <see cref="IMedicationRepository.GetById(long)"/>
        /// </summary>
        public Task<Medication?> GetById(long id) => 
            _connection.Medications.SingleOrDefaultAsync(med => med.Id == id);

        /// <summary>
        /// Implementation of the <see cref="IMedicationRepository.Add(Medication)"/>
        /// </summary>
        public async Task Add(Medication entity)
        {
            await _repository.Add(entity);
        }

        /// <summary>
        /// Implementation of the <see cref="IMedicationRepository.Update(Medication)"/>
        /// </summary>
        public async Task Update(Medication entity)
        {
            await _repository.Update(entity);
        }

        /// <summary>
        /// Implementation of the <see cref="IMedicationRepository.Delete(Medication)"/>
        /// </summary>
        public async Task Delete(Medication entity)
        {
            await _repository.Delete(entity);
        }
    }
}

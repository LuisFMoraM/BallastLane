using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic
{
    public class MedicationService : IMedicationService
    {
        public const string ExistingMedication = "There is already a Medication with the same Name and Brand";
        public const string NonExistingMedication = "The Medication does not exist";

        private readonly IMapper _mapper;
        private readonly IMedicationRepository _medicationRepository;

        public MedicationService(
            IMapper mapper,
            IMedicationRepository medicationRepository)
        {
            _mapper = mapper;
            _medicationRepository = medicationRepository;
        }

        /// <summary>
        /// Implementation of the <see cref="IMedicationService.GetAll"/>
        /// </summary>
        public async Task<List<Medication>> GetAll()
        {
            var entityList = await _medicationRepository.GetAll();
            return _mapper.Map<List<Medication>>(entityList);
        }

        /// <summary>
        /// Implementation of the <see cref="IMedicationService.Add(Medication)"/>
        /// </summary>
        public async Task Add(Medication entity)
        {
            var existingRecord = (await _medicationRepository.GetAll())
                .SingleOrDefault(m => m.Name == entity.Name && m.Brand == entity.Brand);

            if (existingRecord != null)
            {
                throw new InvalidOperationException(ExistingMedication);
            }

            var dbObject = _mapper.Map<DataAccess.Entities.Medication>(entity);
            await _medicationRepository.Add(dbObject);
        }

        /// <summary>
        /// Implementation of the <see cref="IMedicationService.Update(long, Medication)"/>
        /// </summary>
        public async Task Update(long id, Medication entity)
        {
            var existingRecord = await _medicationRepository.GetById(id);
            if (existingRecord is null)
            {
                throw new InvalidOperationException(NonExistingMedication);
            }

            entity.Id = id;
            var dbObject = _mapper.Map<DataAccess.Entities.Medication>(entity);
            await _medicationRepository.Update(dbObject);
        }

        /// <summary>
        /// Implementation of the <see cref="IMedicationService.Delete(long)"/>
        /// </summary>
        public async Task Delete(long id)
        {
            var existingRecord = await _medicationRepository.GetById(id);
            if (existingRecord is null)
            {
                throw new InvalidOperationException(NonExistingMedication);
            }

            await _medicationRepository.Delete(existingRecord);
        }
    }
}
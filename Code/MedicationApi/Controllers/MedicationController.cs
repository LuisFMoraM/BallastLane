using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using MedicationApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MedicationApi.Controllers
{
    /// <summary>
    /// Define Endpoints related to Medications
    /// </summary>
    [ApiController]
    [Route("api/medications")]
    public class MedicationController : ControllerBase
    {
        private readonly ILogger<MedicationController> _logger;
        private readonly IMapper _mapper;
        private readonly IMedicationService _medicationService;

        public MedicationController(
            ILogger<MedicationController> logger,
            IMapper mapper,
            IMedicationService medicationService)
        {
            _logger = logger;
            _mapper = mapper;
            _medicationService = medicationService;
        }

        /// <summary>
        /// Gets all the existing Medications from the system
        /// </summary>
        /// <returns>Medication List</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MedicationDto))]
        public async Task<IActionResult> GetAll()
        {
            var records = await _medicationService.GetAll();
            return Ok(_mapper.Map<List<MedicationDto>>(records));
        }

        /// <summary>
        /// Adds a new Medication to the system
        /// </summary>
        /// <param name="medication">Medication info</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Add(MedicationDto medication)
        {
            var errorMsg = $"Could not Add a new Medication into the system. " +
                $"Sent data: {JsonConvert.SerializeObject(medication)}.";

            try
            {
                var record = _mapper.Map<Medication>(medication);
                await _medicationService.Add(record);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, errorMsg);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMsg);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a Medication in the system
        /// </summary>
        /// <param name="id">Medication identifier</param>
        /// <param name="medication">Medication info</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(long id, MedicationDto medication)
        {
            var errorMsg = $"Could not Update a Medication in the system. " +
                $"Sent data: {JsonConvert.SerializeObject(medication)}.";

            try
            {
                var record = _mapper.Map<Medication>(medication);
                await _medicationService.Update(id, record);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, errorMsg);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMsg);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes a Medication from the system
        /// </summary>
        /// <param name="id">Medication identifier</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(long id)
        {
            var errorMsg = $"Could not Delete a Medication from the system. " +
                $"Sent data: Id {id}.";

            try
            {
                await _medicationService.Delete(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, errorMsg);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMsg);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

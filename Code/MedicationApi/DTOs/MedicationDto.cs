using System.ComponentModel.DataAnnotations;

namespace MedicationApi.DTOs
{
    /// <summary>
    /// Exposes data for a Medication 
    /// </summary>
    public class MedicationDto
    {
        /// <summary>
        /// Gets or sets the medication's identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the medication's name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the medication's brand
        /// </summary>
        [Required]
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the medication's price
        /// </summary>
        [Required]
        public decimal Price { get; set; }
    }
}

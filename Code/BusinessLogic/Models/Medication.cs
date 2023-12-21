namespace BusinessLogic.Models
{
    /// <summary>
    /// Exposes data for a Medication 
    /// </summary>
    public class Medication
    {
        /// <summary>
        /// Gets or sets the medication's identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the medication's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the medication's brand
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the medication's price
        /// </summary>
        public decimal Price { get; set; }
    }
}

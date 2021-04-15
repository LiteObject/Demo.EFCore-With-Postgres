namespace Demo.EFCoreWithPostgres.Domain
{
    /// <summary>
    /// The product.
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        public double UnitPrice { get; set; }
    }
}

namespace Domain.Entities
{
    public class Product : BaseAuditableEntityConcurrent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public decimal Rate { get; set; }

    }
}

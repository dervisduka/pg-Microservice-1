namespace Domain.Events
{
    public class ProductDeletedEvent : BaseEvent
    {
        public ProductDeletedEvent(Product _product)
        {
            Prod = _product;
        }

        public Product Prod { get; }
    }
}

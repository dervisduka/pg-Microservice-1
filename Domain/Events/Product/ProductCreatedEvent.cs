namespace Domain.Events
{
    public class ProductCreatedEvent : BaseEvent
    {
        public ProductCreatedEvent(Product _product)
        {
            Prod = _product;
        }

        public Product Prod { get; }
    }
}

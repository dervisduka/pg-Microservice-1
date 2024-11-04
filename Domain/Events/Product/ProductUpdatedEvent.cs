namespace Domain.Events
{
    public class ProductUpdatedEvent : BaseEvent
    {
        public ProductUpdatedEvent(Product _product)
        {
            Prod = _product;
        }

        public Product Prod { get; }
    }
}

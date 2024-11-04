namespace Domain.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public OrderCreatedEvent(Order _Order)
        {
            Ord = _Order;
        }

        public Order Ord { get; }
    }
}

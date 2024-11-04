namespace Domain.Events
{
    public class OrderUpdatedEvent : BaseEvent
    {
        public OrderUpdatedEvent(Order _Order)
        {
            Prod = _Order;
        }

        public Order Prod { get; }
    }
}

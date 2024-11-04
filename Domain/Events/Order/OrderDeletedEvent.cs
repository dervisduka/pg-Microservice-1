namespace Domain.Events
{
    public class OrderDeletedEvent : BaseEvent
    {
        public OrderDeletedEvent(Order _Order)
        {
            Prod = _Order;
        }

        public Order Prod { get; }
    }
}

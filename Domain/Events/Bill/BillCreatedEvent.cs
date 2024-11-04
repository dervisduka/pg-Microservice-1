namespace Domain.Events
{
    public class BillCreatedEvent : BaseEvent
    {
        public BillCreatedEvent(Bill _Bill)
        {
            Bill = _Bill;
        }

        public Bill Bill { get; }
    }
}

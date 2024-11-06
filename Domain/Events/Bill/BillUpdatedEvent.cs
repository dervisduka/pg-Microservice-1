namespace Domain.Events
{
    public class BillUpdatedEvent : BaseEvent
    {
        public BillUpdatedEvent(Bill _Bill)
        {
            Prod = _Bill;
        }

        public Bill Prod { get; }
    }
}

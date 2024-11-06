namespace Domain.Events
{
    public class BillDeletedEvent : BaseEvent
    {
        public BillDeletedEvent(Bill _Bill)
        {
            Prod = _Bill;
        }

        public Bill Prod { get; }
    }
}

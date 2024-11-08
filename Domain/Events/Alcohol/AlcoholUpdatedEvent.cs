namespace Domain.Events
{
    public class AlcoholUpdatedEvent : BaseEvent
    {
        public AlcoholUpdatedEvent(Alcohol _Alcohol)
        {
            Prod = _Alcohol;
        }

        public Alcohol Prod { get; }
    }
}

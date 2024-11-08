namespace Domain.Events
{
    public class AlcoholCreatedEvent : BaseEvent
    {
        public AlcoholCreatedEvent(Alcohol _Alcohol)
        {
            Ord = _Alcohol;
        }

        public Alcohol Ord { get; }
    }
}

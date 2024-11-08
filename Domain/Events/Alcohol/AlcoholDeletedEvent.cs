namespace Domain.Events
{
    public class AlcoholDeletedEvent : BaseEvent
    {
        public AlcoholDeletedEvent(Alcohol _Alcohol)
        {
            Prod = _Alcohol;
        }

        public Alcohol Prod { get; }
    }
}

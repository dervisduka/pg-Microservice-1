
namespace Application.Entities.Bills.EventHandlers
{
    internal class AlcoholCreatedMessage
    {
        public object Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
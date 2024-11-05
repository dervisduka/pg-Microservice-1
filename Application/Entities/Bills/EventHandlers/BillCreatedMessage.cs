
namespace Application.Entities.Bills.EventHandlers
{
    internal class BillCreatedMessage
    {
        public object Id { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
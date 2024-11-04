namespace Domain.Common
{
    public abstract class BaseEntityConcurrent
    {
        public int Id { get; set; }
        public byte[] timestamp { get; set; }
    }
}

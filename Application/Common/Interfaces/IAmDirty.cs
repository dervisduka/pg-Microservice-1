namespace Application.Common.Interfaces
{
    public interface IAmDirty
    {
        public bool IsDelete { get; set; }
        public bool IsUpdate { get; set; }
    }
}

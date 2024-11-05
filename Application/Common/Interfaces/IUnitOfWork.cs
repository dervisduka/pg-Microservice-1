
using Application.Common.Interfaces;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IProduct2Repository Products2 { get; }

        IBillRepository Bill { get; }
        Task<bool> CommitThings(bool forceNewConnection = false);

    }
}

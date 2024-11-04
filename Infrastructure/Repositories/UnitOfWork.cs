using Application.Common.Interfaces;

using Infrastructure.Persistence;
using MediatR;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMediator _mediator;
        private readonly DbSession _session;

        public IProductRepository Products { get; }
        public IProduct2Repository Products2 { get; }

        public UnitOfWork(
            DbSession session,
            IMediator mediator,
            IProductRepository productRepository,
            IProduct2Repository product2Repository)


        {

            _session = session;
            _mediator = mediator;
            Products = productRepository;
            Products2 = product2Repository;
        }


        public async Task<bool> CommitThings(bool forceNewConnection = false)
        {
            try
            {
                //await _mediator.DispatchDomainEvents(this);
                _session.Transaction.Commit();
                if (forceNewConnection)
                {
                    //support continues transactional.
                    _session.StartNewTransaction();
                }
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not commit the transaction, reason: {e.Message}");
                _session.Transaction.Rollback();
                return false;
            }
            finally
            {
                _session.Transaction.Dispose();
            }
        }

        #region Koment_nqs_do_behet_UnitOfWork_disposable
        //public void Rollback()
        //{
        //    try
        //    {
        //        _session.Transaction.Rollback();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine($"Could not Rollback the transaction, reason: {e.Message}");
        //        _session.Transaction.Dispose();
        //    }
        //    finally
        //    {
        //        _session.Transaction.Dispose();
        //    }

        //}
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _session.Transaction?.Dispose();
        //        _session.Connection?.Dispose();
        //    }
        //}
        #endregion


    }
}

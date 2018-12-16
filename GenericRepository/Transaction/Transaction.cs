
namespace GenericRepository.Transaction
{
    using Microsoft.EntityFrameworkCore.Storage;

    public class Transaction: ITransaction
    {
        private readonly IDbContextTransaction _efTransaction;

        public Transaction(IDbContextTransaction efTransaction)
        {
            _efTransaction = efTransaction;
        }

        public void Commit()
        {
            _efTransaction.Commit();
        }

        public void Rollback()
        {
            _efTransaction.Rollback();
        }

        public void Dispose()
        {
            _efTransaction.Dispose();
        }
    }
}

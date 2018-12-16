
namespace GenericRepository.Transaction
{
    using System;

    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}

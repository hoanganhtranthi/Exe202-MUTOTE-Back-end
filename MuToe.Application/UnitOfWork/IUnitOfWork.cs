

using MuTote.Application.Repository;

namespace MuTote.Application.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<T> Repository<T>() where T : class;

        int Commit();
        Task<int> CommitAsync();
    }
}

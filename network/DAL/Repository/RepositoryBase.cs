using System;
using network.BLL.EF;

namespace network.DAL.Repository
{
    public class RepositoryBase : IDisposable
    {
        protected NetworkContext context;

        public RepositoryBase(NetworkContext con)
        {
            context = con;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposed)
        {
            if (!this.disposed)
            {
                if (disposed)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
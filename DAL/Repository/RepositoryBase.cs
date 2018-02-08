using System;
using DAL.EF;

namespace DAL.Repository
{
    public class RepositoryBase : IDisposable
    {
        protected NetworkContext _context;

        public RepositoryBase(NetworkContext con)
        {
            _context = con;
        }

        public RepositoryBase()
        {
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposed)
        {
            if (!this.disposed)
            {
                if (disposed)
                {
                    _context.Dispose();
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
            _context.SaveChanges();
        }
    }
}
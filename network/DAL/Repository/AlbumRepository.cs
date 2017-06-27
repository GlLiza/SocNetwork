using network.DAL.IRepository;
using System;
using System.Linq;
using network.BLL.EF;
using System.Data.Entity;

namespace network.DAL.Repository
{
    public class AlbumRepository : IAlbumRepository
    {
        private NetworkContext context;

        public AlbumRepository(NetworkContext con)
        {
            context = con;
        }

        public void AddNewAlbum(Photoalbum album)
        {
            context.Photoalbum.Add(album);
        }

        public void DeleteAlbum(Photoalbum alb)
        {
            Photoalbum album = context.Photoalbum.Find(alb.Id);
            context.Photoalbum.Remove(album);
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

        public void UpdateAlbum(Photoalbum album)
        {

            context.Entry(album).State = EntityState.Unchanged;

            //Photoalbum alb = context.Photoalbum.Find(album.Id);
            //context.Entry(alb).CurrentValues.SetValues(album);
        }


        public Photoalbum GetAlbumById(int id)
        {
            return context.Photoalbum.Find(id);
        }
    }
}
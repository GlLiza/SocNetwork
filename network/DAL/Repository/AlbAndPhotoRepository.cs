using System;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class AlbAndPhotoRepository : IAlbAndPhotoRepository
    {
        private NetworkContext context;

        public AlbAndPhotoRepository(NetworkContext con)
        {
            context = con;
        }

        public void AddNewEntry(AlbAndPhot alb)
        {
            context.AlbAndPhot.Add(alb); 
        }

        public void DeleteEntry(AlbAndPhot alb)
        {
            AlbAndPhot album = context.AlbAndPhot.Find(alb.Id);
            context.AlbAndPhot.Remove(album);
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

        public AlbAndPhot GetEntryById(int id)
        {
            return context.AlbAndPhot.Find(id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateEntry(AlbAndPhot album)
        {
            AlbAndPhot alb = context.AlbAndPhot.Find(album.Id);
            context.Entry(alb).CurrentValues.SetValues(album);
        }


        public AlbAndPhot GetEntryByPhotoId (int id)
        {
            var entry = context.AlbAndPhot
                .FirstOrDefault(s => s.ImageId == id);
            return entry;
        }
        
    }
}
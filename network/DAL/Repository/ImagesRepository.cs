using System;
using System.Data.Entity;
using System.Linq;
using network.BLL;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class ImagesRepository : IImagesRepository
    {
        private NetworkContext context;

        public ImagesRepository(NetworkContext cont)
        {
            context = cont;
        }


        public void AddImage(Images images)
        {
            context.Images.Add(images);
        }

        public void DeleteImage(int id)
        {
            Images img = context.Images.Find(id);
               
            context.Images.Remove(img);
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



        public IQueryable<Images> GetImages()
        {
            return context.Images;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateImage(Images images)
        {
            context.Entry(images).State=EntityState.Modified;
        }

        public Images GetImageById(int? id)
        {
            var item = context.Images.Find(id);
            return item;
        }

        public byte[] ReturnImage(string id)
        {
            var img = context.Images.Find(id);
            return img.Data;
        }
    }
}
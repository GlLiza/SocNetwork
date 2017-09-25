using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class ImagesRepository : RepositoryBase,IImagesRepository
    {
       
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

            if(img!=null)
                context.Images.Remove(img);
        }

        public IQueryable<Images> GetImages()
        {
            return context.Images;
        }

        public void UpdateImage(Images images)
        {
            context.Entry(images).State=EntityState.Modified;
        }

        public Images GetImageById(int? id)
        {
            var item = context.Images
                .SingleOrDefault(s=>s.Id==id);
            return item;
        }

        public byte[] ReturnImage(string id)
        {
            var img = context.Images.Find(id);
            return img.Data;
        }

        //сравнивает дату добавления изображения с текущей датой
        public Images CompareDate(List<Images> list)
        {
            var t = list.OrderByDescending(x => x.Date).FirstOrDefault();
            return t;
        }


    }
}
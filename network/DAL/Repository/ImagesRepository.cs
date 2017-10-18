using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class ImagesRepository : RepositoryBase,IImagesRepository
    {

        public ImagesRepository()
        {
        }

        public ImagesRepository(NetworkContext cont):base(cont)
        {
        }


        public void AddImage(Images images)
        {
            _context.Images.Add(images);
            base.Save();
        }

        public void DeleteImage(int id)
        {
            Images img = _context.Images.Find(id);

            if(img!=null)
                _context.Images.Remove(img);

            base.Save();
        }

        public IEnumerable<Images> GetImages()
        {
            return _context.Images;
        }

        public void UpdateImage(Images images)
        {
            _context.Entry(images).State=EntityState.Modified;
            base.Save();
        }

        public Images GetImageById(int? id)
        {
            var item = _context.Images
                .SingleOrDefault(s=>s.Id==id);
            return item;
        }

        public byte[] ReturnImage(string id)
        {
            var img = _context.Images.Find(id);
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
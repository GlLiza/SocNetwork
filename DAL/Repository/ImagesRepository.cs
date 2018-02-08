using System.Collections.Generic;
using System.Linq;
using DAL.IRepository;
using DAL.EF;
using System.Data.Entity;

namespace DAL.Repository
{
    public class ImagesRepository : RepositoryBase,IImagesRepository
    {

        public ImagesRepository()
        {
        }

        public ImagesRepository(NetworkContext cont):base(cont)
        {
        }

        public void Add(Images images)
        {
            _context.Images.Add(images);
            base.Save();
        }

        public void Delete(int id)
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

        public void Update(Images images)
        {
            _context.Entry(images).State=EntityState.Modified;
            base.Save();
        }

        public Images GetById(int? id)
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

        //compare date adding of image with current date
        public Images CompareDate(List<Images> list)
        {
            var t = list.OrderByDescending(x => x.Date).FirstOrDefault();
            return t;
        }
        
    }
}
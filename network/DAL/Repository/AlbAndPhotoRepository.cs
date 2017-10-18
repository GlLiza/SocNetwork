using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class AlbAndPhotoRepository : RepositoryBase,IAlbAndPhotoRepository
    {

        public AlbAndPhotoRepository()
        {
        }


        public AlbAndPhotoRepository(NetworkContext cont):base(cont)
        {
            _context = cont;
        }

        public void AddNewEntry(AlbAndPhot alb)
        {
            _context.AlbAndPhot.Add(alb);
            base.Save();
        }

        public void DeleteEntry(AlbAndPhot alb)
        {
            AlbAndPhot album = _context.AlbAndPhot.Find(alb.Id);
            _context.AlbAndPhot.Remove(album);
            base.Save();
        }
        
        public AlbAndPhot GetEntryById(int id)
        {
            return _context.AlbAndPhot.Find(id);
        }
        
        public void UpdateEntry(AlbAndPhot album)
        {
            AlbAndPhot alb = _context.AlbAndPhot.Find(album.Id);
            _context.Entry(alb).CurrentValues.SetValues(album);
            base.Save();
        }
        
        public AlbAndPhot GetEntryByPhotoId (int id)
        {
            var entry = _context.AlbAndPhot
                .FirstOrDefault(s => s.ImageId == id);
            return entry;
        } 

        public List<Images> GetPhotosFromAlbums(int albumsId)
        {
            var photos = _context.AlbAndPhot.Where(x => x.PhotoalbumId == albumsId).Select(i => i.Images).ToList();
            return photos;
        }

        public AlbAndPhot GetEntry(int albumId, int imageId)
        {
            return _context.AlbAndPhot.SingleOrDefault(q => q.Photoalbum.Id == albumId && q.ImageId == imageId);
        }

        public IQueryable<AlbAndPhot> ReturnEntriesByAlbumId(Photoalbum album)
        {
            var item = _context.AlbAndPhot.Where(q => q.PhotoalbumId == album.Id);
            return item;
        }


        ////возвращает список фотографий соответствующих entrys
        public List<Images> GetArrayImageByEntry(IQueryable<AlbAndPhot> entrys)
        {
            List<Images> arrayImg = new List<Images>();

            foreach (var a in entrys)
            {
                var ph = _context.Images
                   .FirstOrDefault(r => r.Id == a.ImageId);

                arrayImg.Add(ph);
            }
            return arrayImg;
        }

        //получает entry по id альбома и id изображения
        public AlbAndPhot GetEntryByIds(int albumId, int imageId)
        {
            return _context.AlbAndPhot.SingleOrDefault(q => q.Photoalbum.Id == albumId && q.ImageId == imageId);
        }


        public List<Images> GetProfImg(UserDetails user)
        {
            var profPhot = _context.Images
                .Where(q => q.Id == user.ImagesId).ToList();

            return profPhot;
        }

    }
}




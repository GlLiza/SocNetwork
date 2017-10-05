using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class AlbAndPhotoRepository : RepositoryBase,IAlbAndPhotoRepository
    {

        public AlbAndPhotoRepository(NetworkContext cont):base(cont)
        {
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
        
        public AlbAndPhot GetEntryById(int id)
        {
            return context.AlbAndPhot.Find(id);
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



        public List<Images> GetPhotosFromAlbums(int albumsId)
        {
            var photos = context.AlbAndPhot.Where(x => x.PhotoalbumId == albumsId).Select(i => i.Images).ToList();
            return photos;
        }


    }
}
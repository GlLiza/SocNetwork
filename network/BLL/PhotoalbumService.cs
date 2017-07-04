using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace network.BLL
{
    public class PhotoalbumService
    {
        private NetworkContext db = new NetworkContext();

        private IAlbumRepository albumRepository;
        private IAlbAndPhotoRepository AlbAndPhRepository;


        public PhotoalbumService()
        {
            albumRepository = new AlbumRepository(db);
            AlbAndPhRepository=new AlbAndPhotoRepository(db);
        }

        public IQueryable<Photoalbum> GetListAlbums(int id)
        {

            var user = db.UserDetails.Find(id);
            var albums = db.Photoalbum
                .Where(s => s.UserId == id);
            return albums;
        }

        public void AddNewAlbum(Photoalbum alb)
        {
            albumRepository.AddNewAlbum(alb);
            albumRepository.Save();
        }

        public void DeleteAlbum(Photoalbum alb)
        {
            Photoalbum album = albumRepository.GetAlbumById(alb.Id);
            albumRepository.DeleteAlbum(album);
            albumRepository.Save();
        }

        public void EditAlbum(Photoalbum alb)
        {
            albumRepository.UpdateAlbum(alb);
            albumRepository.Save();
        }

        public Photoalbum SearchAlbum(int id)
        {
           var album=albumRepository.GetAlbumById(id);
            return album;
        }

        public void AddNewEntry(AlbAndPhot entr)
        {
           AlbAndPhRepository.AddNewEntry(entr);
          AlbAndPhRepository.Save();

        }




        public IEnumerable<Images> OpenAlbum(int id)
        {
            List<Images> arrayImg = new List<Images>();
            var album = albumRepository.GetAlbumById(id);



            var item = db.AlbAndPhot
                .Where(q => q.PhotoalbumId == album.Id);
            foreach (var a in item)
            {
                var ph = db.Images
                    .FirstOrDefault(r => r.Id == a.ImageId);

                arrayImg.Add(ph);
            }

           return arrayImg;
        }
    }
}
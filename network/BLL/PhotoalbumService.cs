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
        private IImagesRepository imgRepository;


        public PhotoalbumService()
        {
            albumRepository = new AlbumRepository(db);
            AlbAndPhRepository=new AlbAndPhotoRepository(db);
            imgRepository=new ImagesRepository(db);
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
            

            var item = GetListEntry(album);

            var photos = GetArrayImg(item);
           
            foreach (var a in photos)
            {
                var img = imgRepository.GetImageById(a.Id);

                var entry = db.AlbAndPhot
                        .First(q => q.PhotoalbumId == album.Id);

                imgRepository.DeleteImage(img.Id);
                AlbAndPhRepository.DeleteEntry(entry);
                imgRepository.Save();
                AlbAndPhRepository.Save();

            }
           

           

            //foreach (var k in item)
            //{
            //    AlbAndPhRepository.DeleteEntry(k);
            //    AlbAndPhRepository.Save();
            //}

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

        public AlbAndPhot SearcAlbAndPhot(int id)
        {
            return AlbAndPhRepository.GetEntryById(id);
        }

        public void EditAlbAndPhot(AlbAndPhot alb)
        {
            AlbAndPhRepository.UpdateEntry(alb);
            AlbAndPhRepository.Save();
        }


        public void DeleteAlbAndPhot(AlbAndPhot alb)
        {
            AlbAndPhRepository.DeleteEntry(alb);
            AlbAndPhRepository.Save();
        }

        public IQueryable<AlbAndPhot> GetListEntry(Photoalbum alb)
        {
            Photoalbum album = albumRepository.GetAlbumById(alb.Id);

            var item = db.AlbAndPhot
               .Where(q => q.PhotoalbumId == album.Id);
            return item;
        }

        public List<Images> GetArrayImg(IQueryable<AlbAndPhot> entrys)
        {
            List<Images> arrayImg = new List<Images>();
            foreach (var a in entrys)
            {
                var ph = db.Images
                   .FirstOrDefault(r => r.Id == a.ImageId);

                arrayImg.Add(ph);
            }
            return arrayImg;
        }




        public List<Images> OpenAlbum(int id)
        {
            var album = albumRepository.GetAlbumById(id);

            var item = GetListEntry(album);
            var imgs= GetArrayImg(item);

           return imgs;
        }

        public void DeletePhoto(Images img)
        {
            var images = imgRepository.GetImageById(img.Id);
            var entry = AlbAndPhRepository.GetEntryByPhotoId(images.Id);
            DeleteAlbAndPhot(entry);
            imgRepository.DeleteImage(images.Id);
            imgRepository.Save();

        }
    }
}
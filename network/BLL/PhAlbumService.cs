using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;
using System.Collections.Generic;
using System.Linq;

namespace network.BLL
{
    public class PhAlbumService
    {
        private NetworkContext db = new NetworkContext();

        private IAlbumRepository albumRepository;
        private IAlbAndPhotoRepository AlbAndPhRepository;
        private IImagesRepository imgRepository;


        public PhAlbumService()
        {
            albumRepository = new AlbumRepository(db);
            AlbAndPhRepository=new AlbAndPhotoRepository(db);
            imgRepository=new ImagesRepository(db);
        }




        //PHOTOALBUM
        public IQueryable<Photoalbum> GetListAlbums(int id)
        {
            return albumRepository.GetListAlbums(id);
        }
        
        public void AddNewAlbum(Photoalbum alb)
        {
            albumRepository.AddNewAlbum(alb);
            albumRepository.Save();
        }

        public void DeleteAlbum(Photoalbum alb)
        {
            
            Photoalbum album = albumRepository.GetAlbumById(alb.Id);
            

            var item = GetListEntry(album.Id);

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
            return albumRepository.GetAlbumById(id);
        }

        public IQueryable<Photoalbum> GetAlbums(int id)
        {
            return albumRepository.GetListAlbums(id);
        }



        
        
        //ALBUM_AND_PHOTOS

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

        public IQueryable<AlbAndPhot> GetListEntry(int id)
        {
            Photoalbum album = albumRepository.GetAlbumById(id);

            var item = db.AlbAndPhot
               .Where(q => q.PhotoalbumId == album.Id);

            return item;
        }

        //возвращает список фотографий соответствующих entrys
        public List<Images> GetArrayImg(IQueryable<AlbAndPhot> entrys)
        {
            List<Images> arrayImg = new List<Images>();

            foreach (var a in entrys)
            {
                var ph = db.Images
                   .FirstOrDefault(r => r.Id == a.ImageId);

                arrayImg.Add(ph);
            }
            arrayImg.Sort((x, y) =>y.Date.Value.CompareTo(x.Date.Value) );
            return arrayImg;
        }
        



        //IMAGES

       //получает список фотографий альбома
        public List<Images> OpenAlbum(int id)
        {
            var item = GetListEntry(id);
            return GetArrayImg(item);
        }
        
        //удаляет фото из альбома
        public void DeletePhoto(int id)
        {
            var images = imgRepository.GetImageById(id);
            var entry = AlbAndPhRepository.GetEntryByPhotoId(images.Id);

            DeleteAlbAndPhot(entry);

            imgRepository.DeleteImage(images.Id);
            imgRepository.Save();

        }
        
        //возdращает список профильных изобр
        public List<Images> GetProfImgId (UserDetails user)
        {
            var profPhot = db.Images
                .Where(q => q.Id == user.ImagesId).ToList();

            return profPhot;
        }
        


        //возвращает все изображения пользователя
        public List<Images> GetAllImg (int id)
        {
            UserDetails user = db.UserDetails.Find(id);

            List<Images> all=new List<Images>();

            var profImg = GetProfImgId(user);

            foreach (var img in profImg)
            {
                all.Add(img);
            }

            var albums = GetAlbums(id);
            foreach (var alb in albums)
            {
                var item = GetListEntry(alb.Id);
                var a = GetArrayImg(item);
                foreach (var aa in a)
                {
                    all.Add(aa);
                }
              

            }

            return all;
        }


    }
}
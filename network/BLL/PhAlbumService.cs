using System;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;
using System.Collections.Generic;
using System.Linq;

namespace network.BLL
{
    public class PhAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IAlbAndPhotoRepository _albAndPhRepository;
        private readonly IImagesRepository _imgRepository;
        private readonly IUserRepository _userRepository;

        //public PhAlbumService()
        //{
        //}

        public PhAlbumService(AlbumRepository albumRepository, AlbAndPhotoRepository albAndPhotoRepository,
            ImagesRepository imgRepository,UserRepository userRepository)
        {
            _albumRepository = albumRepository;
            _albAndPhRepository = albAndPhotoRepository;
            _imgRepository = imgRepository;
            _userRepository = userRepository;
        }




        //PHOTOALBUM
        public IQueryable<Photoalbum> GetListAlbums(int id)
        {
            return _albumRepository.GetListAlbums(id);
        }

        public void AddNewAlbum(Photoalbum alb)
        {
            _albumRepository.AddNewAlbum(alb);
        }
        
        public bool DeleteAlbum(int albId)
        {
            if (albId <= 0) throw new ArgumentOutOfRangeException(nameof(albId));
            {
            
            Photoalbum album = _albumRepository.GetAlbumById(albId);
            var item = GetListEntry(album.Id); //получаем список записей (альб ->изобр)

            if (item.Count() != 0)
            {
                var photos = GetArrayImg(item); //получаем список изобр альбома

                foreach (var a in photos)
                {
                    var entry = _albAndPhRepository.GetEntry(album.Id, a.Id);

                    _imgRepository.DeleteImage(a.Id);
                    _albAndPhRepository.DeleteEntry(entry);
                }

                _albumRepository.DeleteAlbum(album.Id);
            }
                return true;
            }
        }

        public IQueryable<AlbAndPhot> GetListEntry(int id)
        {
            Photoalbum album = _albumRepository.GetAlbumById(id);

            var item = _albAndPhRepository.ReturnEntriesByAlbumId(album);

            return item;
        }
        
        //получает запись по id альбома и id изображения
        public AlbAndPhot GetEntryByIds(int albumId, int imageId)
        {
            return _albAndPhRepository.GetEntryByIds(albumId,imageId);
        }
        
        public void EditAlbum(Photoalbum alb)
        {
            _albumRepository.UpdateAlbum(alb);
        }

        public Photoalbum SearchAlbum(int id)
        {
            return _albumRepository.GetAlbumById(id);
        }

        public IQueryable<Photoalbum> GetAlbums(int id)
        {
            return _albumRepository.GetListAlbums(id);
        }
        
        //ALBUM_AND_PHOTOS

        public void AddNewEntry(AlbAndPhot entr)
        {
          _albAndPhRepository.AddNewEntry(entr);
        }

        public AlbAndPhot SearcAlbAndPhot(int id)
        {
            return _albAndPhRepository.GetEntryById(id);
        }

        public void EditAlbAndPhot(AlbAndPhot alb)
        {
            _albAndPhRepository.UpdateEntry(alb);
        }


        public void DeleteAlbAndPhot(AlbAndPhot alb)
        {
            _albAndPhRepository.DeleteEntry(alb);
        }

        ////возвращает список фотографий соответствующих entrys и сортирует их по дате
        public List<Images> GetArrayImg(IQueryable<AlbAndPhot> entrys)
        {
            var arrayImg = _albAndPhRepository.GetArrayImageByEntry(entrys);
            arrayImg.Sort((x, y) =>y.Date.Value.CompareTo(x.Date.Value));
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
            if (id != 0)
            {
                var images = _imgRepository.GetImageById(id);
                if (images != null)
                {
                    var entry = _albAndPhRepository.GetEntryByPhotoId(images.Id);

                    DeleteAlbAndPhot(entry);

                    _imgRepository.DeleteImage(images.Id);
                }
            }
           

        }
        
        //возdращает список профильных изобр
        public List<Images> GetProfImgId (UserDetails user)
        {
            return _albAndPhRepository.GetProfImg(user);
        }
        


        //возвращает все изображения пользователя
        public List<Images> GetAllImg (int id)
        {
            var user = _userRepository.GetUserById(id);

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
        

        public Images GetLastImg(int albumId)
        {
            List<Images> imgs = OpenAlbum(albumId);

            Images lastImg = _imgRepository.CompareDate(imgs);
            return lastImg;
        }


        public List<Images> AlbumImg(List<Photoalbum> albums)
        {
            List<Images> imgs = new List<Images>();

            foreach (var alb in albums)
            {
                var img = GetLastImgAlbum(alb.Id);
                imgs.Add(img);
            }
            return imgs;
        }


        public Images GetLastImgAlbum(int albumId)
        {
            var allPhoto = _albAndPhRepository.GetPhotosFromAlbums(albumId);
            Images img = _imgRepository.CompareDate(allPhoto);
            return img;
        }

    }
}
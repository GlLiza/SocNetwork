using System;
using DAL.Repository;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;
using DAL.IRepository;

namespace BLL
{
    public class PhAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IAlbAndPhotoRepository _albAndPhRepository;
        private readonly IImagesRepository _imgRepository;
        private readonly IUserRepository _userRepository;

      
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
            _albumRepository.Add(alb);
        }
        
        public bool DeleteAlbum(int albId)
        {
            if (albId <= 0) throw new ArgumentOutOfRangeException(nameof(albId));
            {
            
            Photoalbum album = _albumRepository.GetById(albId);
            var item = GetListEntry(album.Id); //получаем список записей (альб ->изобр)

            if (item.Count() != 0)
            {
                var photos = GetArrayImg(item); //получаем список изобр альбома

                foreach (var a in photos)
                {
                    var entry = _albAndPhRepository.GetEntry(album.Id, a.Id);

                    _imgRepository.Delete(a.Id);
                    _albAndPhRepository.Delete(entry);
                }

                _albumRepository.Delete(album.Id);
            }
                return true;
            }
        }

        public void EditAlbum(Photoalbum alb)
        {
            _albumRepository.Update(alb);
        }

        public Photoalbum SearchAlbum(int id)
        {
            return _albumRepository.GetById(id);
        }

        public IQueryable<Photoalbum> GetAlbums(int id)
        {
            return _albumRepository.GetListAlbums(id);
        }


        public IQueryable<AlbAndPhot> GetListEntry(int id)
        {
            Photoalbum album = _albumRepository.GetById(id);
            var item = _albAndPhRepository.ReturnEntriesByAlbumId(album);
            return item;
        }
        
        
        
        //ALBUM_AND_PHOTOS

        public void AddNewEntry(AlbAndPhot entr)
        {
          _albAndPhRepository.Add(entr);
        }
        public void DeleteAlbAndPhot(AlbAndPhot alb)
        {
            _albAndPhRepository.Delete(alb);
        }

        //get entry by id of albums and id of image
        public AlbAndPhot GetEntryByIds(int albumId, int imageId)
        {
            return _albAndPhRepository.GetEntryByIds(albumId, imageId);
        }

        //get list of images for list entries and sort them
        public List<Images> GetArrayImg(IQueryable<AlbAndPhot> entrys)
        {
            var arrayImg = _albAndPhRepository.GetArrayImageByEntry(entrys);
            arrayImg.Sort((x, y) =>y.Date.Value.CompareTo(x.Date.Value));
            return arrayImg;
        }

        //IMAGES

       //get list of img from album
        public List<Images> OpenAlbum(int id)
        {
            var item = GetListEntry(id);
            return GetArrayImg(item);
        }
        
        //remove img in album
        public void DeletePhoto(int id)
        {
            if (id != 0)
            {
                var images = _imgRepository.GetById(id);
                if (images != null)
                {
                    var entry = _albAndPhRepository.GetByPhotoId(images.Id);

                    DeleteAlbAndPhot(entry);

                    _imgRepository.Delete(images.Id);
                }
            }
           

        }
        
        //return list of profile img
        public List<Images> GetProfImgId (UserDetails user)
        {
            return _albAndPhRepository.GetProfImg(user);
        }
        
        //return all imgs for user 
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

        public Images GetLastImgAlbum(int albumId)
        {
            var allPhoto = _albAndPhRepository.GetPhotosFromAlbums(albumId);
            Images img = _imgRepository.CompareDate(allPhoto);
            return img;
        }

    }
}
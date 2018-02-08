using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.IRepository
{
    public interface IAlbAndPhotoRepository:IDisposable
    {
        void Add(AlbAndPhot alb);
        void Delete(AlbAndPhot album);
        void Update(AlbAndPhot album);
        
        AlbAndPhot GetById(int id);
        AlbAndPhot GetByPhotoId(int id);

        AlbAndPhot GetEntry(int albumId, int imageId);
        IQueryable<AlbAndPhot> ReturnEntriesByAlbumId(Photoalbum album);
        AlbAndPhot GetEntryByIds(int albumId, int imageId);


        List<Images> GetPhotosFromAlbums(int albumsId);
        List<Images> GetArrayImageByEntry(IQueryable<AlbAndPhot> entrys);
        List<Images> GetProfImg(UserDetails user);
    }
}

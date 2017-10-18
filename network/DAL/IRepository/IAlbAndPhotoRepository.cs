using network.BLL.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace network.DAL.IRepository
{
    interface IAlbAndPhotoRepository:IDisposable
    {
        void AddNewEntry(AlbAndPhot alb);
        void DeleteEntry(AlbAndPhot album);
        void UpdateEntry(AlbAndPhot album);
        void Save();
        AlbAndPhot GetEntryById(int id);
        AlbAndPhot GetEntryByPhotoId(int id);
        List<Images> GetPhotosFromAlbums(int albumsId);
        AlbAndPhot GetEntry(int albumId, int imageId);
        IQueryable<AlbAndPhot> ReturnEntriesByAlbumId(Photoalbum album);
        List<Images> GetArrayImageByEntry(IQueryable<AlbAndPhot> entrys);
        AlbAndPhot GetEntryByIds(int albumId, int imageId);
        List<Images> GetProfImg(UserDetails user);

    }
}

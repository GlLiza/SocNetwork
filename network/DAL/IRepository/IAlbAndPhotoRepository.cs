using network.BLL.EF;
using System;
using System.Collections.Generic;

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
    }
}

using network.BLL.EF;
using System;

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

    }
}

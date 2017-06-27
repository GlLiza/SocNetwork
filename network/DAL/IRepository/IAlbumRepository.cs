using network.BLL.EF;
using System;
using System.Linq;

namespace network.DAL.IRepository
{
    interface IAlbumRepository:IDisposable
    {
        void AddNewAlbum(Photoalbum album);
        void DeleteAlbum(Photoalbum album);
        void UpdateAlbum(Photoalbum album);
        void Save();
        Photoalbum GetAlbumById(int id);

       
        
    }
}

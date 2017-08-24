using network.BLL.EF;
using System;
using System.Linq;

namespace network.DAL.IRepository
{
    interface IAlbumRepository
    {
        void AddNewAlbum(Photoalbum album);
        void DeleteAlbum(Photoalbum album);
        void UpdateAlbum(Photoalbum album);
        Photoalbum GetAlbumById(int id);
        IQueryable<Photoalbum> GetListAlbums(int id);
    }
}

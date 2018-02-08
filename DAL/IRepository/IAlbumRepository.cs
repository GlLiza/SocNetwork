using DAL.EF;
using System.Linq;

namespace DAL.IRepository
{
    public interface IAlbumRepository
    {
        void Add(Photoalbum album);
        bool Delete(int albumId);
        void Update(Photoalbum album);

        Photoalbum GetById(int id);
        IQueryable<Photoalbum> GetListAlbums(int id);
    }
}

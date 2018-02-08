using DAL.IRepository;
using System.Linq;
using DAL.EF;

namespace DAL.Repository
{
    public class AlbumRepository : RepositoryBase, IAlbumRepository
    {
        public AlbumRepository()
        {
        }

        public AlbumRepository(NetworkContext cont) : base(cont)
        {
            
        }

        public void Add(Photoalbum album)
        {
            _context.Photoalbum.Add(album);
            base.Save();
        }

        public bool Delete(int albId)
        {
            Photoalbum album = _context.Photoalbum.Find(albId);

            if (album != null)
            {
                _context.Photoalbum.Remove(album);
                base.Save();
                return true;
            }

            return false;


        }

        public void Update(Photoalbum album)
        {
            Photoalbum alb = _context.Photoalbum.Find(album.Id);
            _context.Entry(alb).CurrentValues.SetValues(album);
            base.Save();
        }


        public Photoalbum GetById(int id)
        {
            return _context.Photoalbum.Find(id);
        }

        public IQueryable<Photoalbum> GetListAlbums(int id)
        {
            var albums = _context.Photoalbum
                .Where(s => s.UserId == id)
                .OrderByDescending(s=>s.Id);
            return albums;
        }

    }
}
using network.DAL.IRepository;
using System.Linq;
using network.BLL.EF;

namespace network.DAL.Repository
{
    public class AlbumRepository : RepositoryBase, IAlbumRepository
    {
        public AlbumRepository(NetworkContext con)
        {
            context = con;
        }

        public void AddNewAlbum(Photoalbum album)
        {
            context.Photoalbum.Add(album);
        }

        public bool DeleteAlbum(int albId)
        {
            Photoalbum album = context.Photoalbum.Find(albId);

            if (album != null)
            {
                context.Photoalbum.Remove(album);
                context.SaveChanges();
                return true;
            }

            return false;


        }

        public void UpdateAlbum(Photoalbum album)
        {

            //context.Entry(album).State = EntityState.Unchanged;

            Photoalbum alb = context.Photoalbum.Find(album.Id);
            context.Entry(alb).CurrentValues.SetValues(album);
            context.SaveChanges();
        }


        public Photoalbum GetAlbumById(int id)
        {
            return context.Photoalbum.Find(id);
        }

        public IQueryable<Photoalbum> GetListAlbums(int id)
        {
            var albums = context.Photoalbum
                .Where(s => s.UserId == id)
                .OrderByDescending(s=>s.Id);
            return albums;
        }


    }
}
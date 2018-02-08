using System.Collections.Generic;
using System.Linq;
using DAL.IRepository;
using DAL.EF;

namespace DAL.Repository
{
    public class WorkPlaceRepository : RepositoryBase,IWorkPlaceRepository
    {

        public WorkPlaceRepository() 
        {
        }

        public WorkPlaceRepository(NetworkContext cont):base(cont)
        {
        }

        public void Add(WorkPlace place)
        {
            _context.WorkPlace.Add(place);
            base.Save();
        }

        public void Delete(int placeId)
        {
            WorkPlace place = _context.WorkPlace.Find(placeId);
            _context.WorkPlace.Remove(place);
            base.Save();
        }

        public void Update(WorkPlace place)
        {
            WorkPlace pl = _context.WorkPlace.Find(place.Id);
            _context.Entry(pl).CurrentValues.SetValues(place);
            base.Save();
        }

        public IEnumerable<WorkPlace> GetListWorks(int id)
        {
            var works = _context.WorkPlace.Where(x => x.UserId == id);
            return works;
        }

        public WorkPlace GetPlaseById(int id)
        {
            return _context.WorkPlace.Find(id);
        }
        
    }
}
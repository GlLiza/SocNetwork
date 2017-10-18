using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class WorkPlaceRepository : RepositoryBase,IWorkPlaceRepository
    {

        public WorkPlaceRepository() 
        {
        }

        public WorkPlaceRepository(NetworkContext cont):base(cont)
        {
        }

        public void AddWorkPlace(WorkPlace place)
        {
            _context.WorkPlace.Add(place);
            base.Save();
        }

        public void DeleteWorkPlace(int placeId)
        {
            WorkPlace place = _context.WorkPlace.Find(placeId);
            _context.WorkPlace.Remove(place);
            base.Save();
        }
        
        public IEnumerable<WorkPlace> GetListWorks(int id)
        {
            UserDetails user = _context.UserDetails.Find(id);

            var placesUs = _context.WorkPlace
                .Where(s => s.Id == user.WorkPlaceId);

            return placesUs;
        }

        public WorkPlace GetPlaseById(int id)
        {
            return _context.WorkPlace.Find(id);
        }
        
        public void Update(WorkPlace place)
        {
            WorkPlace pl = _context.WorkPlace.Find(place.Id);
            _context.Entry(pl).CurrentValues.SetValues(place);
            base.Save();
        }
    }
}
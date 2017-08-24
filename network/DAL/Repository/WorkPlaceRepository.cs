using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class WorkPlaceRepository : RepositoryBase,IWorkPlaceRepository
    {
      

        public WorkPlaceRepository(NetworkContext cont)
        {
            context = cont;
        }

        public void AddWorkPlace(WorkPlace place)
        {
            context.WorkPlace.Add(place);
        }

        public void DeleteWorkPlace(int placeId)
        {
            WorkPlace place = context.WorkPlace.Find(placeId);
            context.WorkPlace.Remove(place);
        }
        
        public IEnumerable<WorkPlace> GetListWorks(int id)
        {
            UserDetails user = context.UserDetails.Find(id);

            var placesUs = context.WorkPlace
                .Where(s => s.Id == user.WorkPlaceId);

            return placesUs;
        }

        public WorkPlace GetPlaseById(int id)
        {
            return context.WorkPlace.Find(id);
        }
        
        public void Update(WorkPlace place)
        {
            WorkPlace pl = context.WorkPlace.Find(place.Id);
            context.Entry(pl).CurrentValues.SetValues(place);
        }
    }
}
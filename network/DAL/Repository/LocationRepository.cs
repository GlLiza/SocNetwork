using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class LocationRepository : RepositoryBase,ILocationRepository
    {
     

        public LocationRepository(NetworkContext con)
        {
            context = con;
        }

        public void AddNewLocation(Location location)
        {
            context.Location.Add(location);
        }

        public void DeleteLocation(Location location)
        {
            Location loc = context.Location.Find(location.Id);
            context.Location.Remove(loc);
        }

        public Location GetLocationById(int? id)
        {
            return context.Location.Find(id);
        }


        public void UpdateLocation(Location location)
        {
            Location loc = context.Location.Find(location.Id);
            context.Entry(loc).CurrentValues.SetValues(location);
        }

        public IEnumerable<Location> GetListCurLoc(int? id)
        {
            UserDetails user= context.UserDetails.Find(id);

            var listLocation = context.Location
                .Where(s => s.Id == user.CurrentLocationId);
            return listLocation;
        }
        public IEnumerable<Location> GetListHomeLoc(int? id)
        {
            UserDetails user = context.UserDetails.Find(id);

            var listLocation = context.Location
                .Where(s => s.Id == user.HomeTownLocationId);
            return listLocation;
        }

    }
}
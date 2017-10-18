using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class LocationRepository : RepositoryBase,ILocationRepository
    {

        public LocationRepository()
        {
        }

        public LocationRepository(NetworkContext cont):base(cont)
        {
        }

        public void AddNewLocation(Location location)
        {
            _context.Location.Add(location);
            base.Save();
        }

        public void DeleteLocation(Location location)
        {
            Location loc = _context.Location.Find(location.Id);
            _context.Location.Remove(loc);
            base.Save();
        }

        public Location GetLocationById(int? id)
        {
            return _context.Location.Find(id);
        }


        public void UpdateLocation(Location location)
        {
            Location loc = _context.Location.Find(location.Id);
            _context.Entry(loc).CurrentValues.SetValues(location);
            base.Save();
        }

        public IEnumerable<Location> GetListCurLoc(int? id)
        {
            UserDetails user= _context.UserDetails.Find(id);

            var listLocation = _context.Location
                .Where(s => s.Id == user.CurrentLocationId);
            return listLocation;
        }
        public IEnumerable<Location> GetListHomeLoc(int? id)
        {
            UserDetails user = _context.UserDetails.Find(id);

            var listLocation = _context.Location
                .Where(s => s.Id == user.HomeTownLocationId);
            return listLocation;
        }

    }
}
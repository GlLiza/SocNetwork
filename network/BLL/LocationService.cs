using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class LocationService
    {
        NetworkContext db = new NetworkContext();

        private ILocationRepository locationRepository;

        public LocationService()
        {
            locationRepository=new LocationRepository(db);
        }

        public void AddLocation(Location location)
        {
            locationRepository.AddNewLocation(location);
            locationRepository.Save();
        }

        public void DeleteLocation(Location location)
        {
            Location loc = locationRepository.GetLocationById(location.Id);
            locationRepository.DeleteLocation(loc);
        }

        public Location GetLocation(int id)
        {
            return locationRepository.GetLocationById(id);
        }

        public void UpdateLocation(Location location)
        {
            locationRepository.UpdateLocation(location);
        }


    }
}
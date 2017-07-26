using System;
using System.Collections.Generic;
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

        public Location GetLocation(int? id)
        {
            return locationRepository.GetLocationById(id);
        }

        public void UpdateLocation(Location location)
        {
            locationRepository.UpdateLocation(location);
        }

        public IEnumerable<Location> ListCurrentLoc(int? useId)
        {
            return locationRepository.GetListCurLoc(useId);
        }

        public IEnumerable<Location> ListHomeLoc(int? useId)
        {
            return locationRepository.GetListHomeLoc(useId);
        }


    }
}
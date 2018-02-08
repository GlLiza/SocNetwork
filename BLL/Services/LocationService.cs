using DAL.IRepository;
using DAL.Repository;
using DAL.EF;

namespace BLL
{
    public class LocationService
    {
        private readonly ILocationRepository _locRepository;

        //public LocationService()
        //{
        //}

        public LocationService(LocationRepository locationRepository)
        {
            _locRepository = locationRepository;
        }

        //public void AddLocation(Location location)
        //{
        //    _locRepository.AddNewLocation(location);
        //}

        //public void DeleteLocation(Location location)
        //{
        //    Location loc = _locRepository.GetLocationById(location.Id);
        //    _locRepository.DeleteLocation(loc);
        //}

        public Location GetLocation(int? id)
        {
            return _locRepository.GetLocationById(id);
        }

        //public void UpdateLocation(Location location)
        //{
        //    _locRepository.UpdateLocation(location);
        //}

        //public IEnumerable<Location> ListCurrentLoc(int? useId)
        //{
        //    return _locRepository.GetListCurLoc(useId);
        //}

        //public IEnumerable<Location> ListHomeLoc(int? useId)
        //{
        //    return _locRepository.GetListHomeLoc(useId);
        //}


    }
}
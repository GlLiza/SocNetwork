using System;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface ILocationRepository:IDisposable
    {
        void AddNewLocation(Location location);
        void DeleteLocation(Location location);
        void UpdateLocation(Location location);
        void Save();
        Location GetLocationById(int id);
    }
}

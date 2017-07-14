using System;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private NetworkContext context;

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

        private bool disposed = false;

        protected virtual void Dispose(bool disposed)
        {
            if (!this.disposed)
            {
                if (disposed)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Location GetLocationById(int id)
        {
            return context.Location.Find(id);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateLocation(Location location)
        {
            Location loc = context.Location.Find(location.Id);
            context.Entry(loc).CurrentValues.SetValues(location);
        }
    }
}
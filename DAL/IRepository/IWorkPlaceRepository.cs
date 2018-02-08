using System;
using System.Collections.Generic;
using DAL.EF;

namespace DAL.IRepository
{
    public interface IWorkPlaceRepository : IDisposable
    {
        void Add(WorkPlace place);
        void Delete(int placeId);
        void Update(WorkPlace place);

        WorkPlace GetPlaseById(int id);
        IEnumerable<WorkPlace> GetListWorks(int id);
    }
}

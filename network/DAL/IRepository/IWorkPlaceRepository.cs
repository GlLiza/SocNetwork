using System;
using System.Collections.Generic;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IWorkPlaceRepository : IDisposable
    {
        IEnumerable<WorkPlace> GetListWorks(int id);
        void AddWorkPlace(WorkPlace place);
        void DeleteWorkPlace(int placeId);
        void Update(WorkPlace place);
        void Save();

        WorkPlace GetPlaseById(int id);
    }
}

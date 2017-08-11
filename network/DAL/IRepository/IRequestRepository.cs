using System;
using System.Linq;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IRequestRepository:IDisposable
    {
        void AddRequest(Requests requests);
        void CancelRequests(Requests requests);
        void Update(Requests requests);
        void Save();

        Requests SearchById(int id);
        Requests SearchByUsersId(string idIng, string idEd);
        IQueryable<Requests> GetActiveRequests(string id);
        Requests CheckRequests(string idOne, string idTwo);
    }
}

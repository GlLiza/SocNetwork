using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IRequestRepository:IDisposable
    {
        void NewRequest(Requests requests);
        void CancelRequests(Requests requests);
        void Update(Requests requests);
        void Save();

        Requests SearchById(int id);
        Requests SearchByUsersId(string idIng, string idEd);
        IQueryable<Requests> SearchRequests(string id);
        IQueryable<Requests> ShowNewRequests(string id);

        Requests ReturnRequests(string idOne, string idTwo);
    }
}

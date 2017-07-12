using System;
using System.Collections.Generic;
using System.Linq;
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
        IEnumerable<Requests> SearchRequests(string id);

        

        
    }
}

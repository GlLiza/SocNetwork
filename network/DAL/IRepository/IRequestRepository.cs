using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IRequestRepository:IDisposable
    {
        void NewRequest(Requests friend);
        void CancelRequests(Requests friend);
        void Save();

        Requests SearchById(int id);
        List<Requests> SearchRequests(string id);

        

        
    }
}

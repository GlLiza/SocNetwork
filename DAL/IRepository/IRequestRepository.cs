using System;
using System.Linq;
using DAL.Enums;
using DAL.EF;

namespace DAL.IRepository
{
    public interface IRequestRepository:IDisposable
    {
        void Add(Requests requests);
        void Cancel(Requests requests);
        void Update(Requests requests);
        void EditStatusOfRequest(int id, FriendshipStatus active);

        Requests SearchById(int id);
        Requests SearchByUsersId(string idIng, string idEd);
        IQueryable<Requests> GetActiveRequests(string id);
        Requests CheckRequests(string idOne, string idTwo);
        
    }
}

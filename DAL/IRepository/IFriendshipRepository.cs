using System;
using System.Collections.Generic;
using System.Linq;
using DAL.EF;

namespace DAL.IRepository
{
    public interface IFriendshipRepository:IDisposable
    {
        void Add(Friendship Friendship);
        void Delete(int id);
        bool Check(string uId, string fId);

        Friendship SearchById(int id);
        Friendship SearchByUsers(string idU, string idF);
       
        List<string> GetListFriendsId(string id);
        IQueryable<Friendship> GetListFriends(string id);
    }
}
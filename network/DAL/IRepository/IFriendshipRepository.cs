using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IFriendshipRepository:IDisposable
    {
        void AddFriend(Friendship Friendship);
        void DeleteFriend(int id);
        Friendship SearchByUsers(string idU, string idF);
        Friendship SearchById(int id);
        void Save();
        IQueryable<Friendship> GetListFriends(string id);

        List<string> GetListFriendsId(string id);

        bool Check(string uId, string fId);

    }
}
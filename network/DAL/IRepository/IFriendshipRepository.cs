using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IFriendshipRepository:IDisposable
    {
        void AddFriend(Friendship friend);
        void DeleteFriend(Friendship friend);
        Friendship SearchByCurrentUserId(string id);
        Friendship SearchBySecondUserId(string id);
        Friendship SearchById(int id);
        void Save();
        IQueryable<Friendship> GetListFriends(string id);
    }
}
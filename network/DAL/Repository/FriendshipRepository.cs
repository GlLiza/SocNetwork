using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private NetworkContext context;

        public FriendshipRepository(NetworkContext cont)
        {
            context = cont;
        }

        public void AddFriend(Friendship friend)
        {
            context.Friendship.Add(friend);
        }

        public void DeleteFriend(Friendship friendship)
        {
            Friendship friend = context.Friendship.Find(friendship.Id);
            context.Friendship.Remove(friend);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposed)
        {
            if (!this.disposed)
            {
                if (disposed)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public Friendship SearchByCurrentUserId(string id)
        {
            return context.Friendship
                .SingleOrDefault(s => s.User_id == id);
        }

        public Friendship SearchBySecondUserId(string id)
        {
            return context.Friendship
                .FirstOrDefault(s => s.User_id == id);
        }

        public IQueryable<Friendship> GetListFriends(string id)
        {
            var list = context.Friendship
                .Where(s => s.User_id == id);
            return list;
        }

        public Friendship SearchById(int id)
        {
            return context.Friendship.Find(id);
        }


    }
}
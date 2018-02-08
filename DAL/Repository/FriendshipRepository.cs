using System.Collections.Generic;
using System.Linq;
using DAL.IRepository;
using DAL.EF;

namespace DAL.Repository
{
    public class FriendshipRepository : RepositoryBase, IFriendshipRepository
    {
        public FriendshipRepository()
        {
        }

        public FriendshipRepository(NetworkContext cont) : base(cont)
        {
        }

        
        public void Add(Friendship friendship)
        {
            _context.Friendship.Add(friendship);
            base.Save();
        }

        public void Delete(int id)
        {
            Friendship friendship = _context.Friendship.Find(id);
            if (friendship != null)
                _context.Friendship.Remove(friendship);
            base.Save();
        }

        //function checks there are frindship between users or no
        public bool Check(string uId, string fId)
        {
            var result = _context.Friendship
                .FirstOrDefault(x => x.User_id == uId && x.Friend_id == fId);
            if (result != null)
                return true;
            return false;
        }


        public Friendship SearchById(int id)
        {
            return _context.Friendship.Find(id);
        }

        //return object of class Friendship (between users) by ids of users 
        public Friendship SearchByUsers(string idU, string idF)
        {
            return _context.Friendship
                .FirstOrDefault(s => s.User_id == idF && s.Friend_id == idU);
        }


        //get list of users for user by him id
        public IQueryable<Friendship> GetListFriends(string id)
        {
            var list = _context.Friendship
                .Where(s => s.User_id == id);
            return list;
        }


        //get list ids of friends by id of user for this user 
        public List<string> GetListFriendsId(string id)
        {
            var list = _context.Friendship
                .Where(s => s.User_id == id).Select(i=>i.Friend_id).ToList();
            return list;
        }
        
    }
}
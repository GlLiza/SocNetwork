using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class FriendshipRepository : RepositoryBase, IFriendshipRepository
    {

        public FriendshipRepository()
        {
        }

        public FriendshipRepository(NetworkContext cont) : base(cont)
        {
        }

        

        public void AddFriend(Friendship friendship)
        {
            _context.Friendship.Add(friendship);
            base.Save();
        }

        public void DeleteFriend(int id)
        {
            Friendship friendship = _context.Friendship.Find(id);
            if (friendship != null)
                _context.Friendship.Remove(friendship);
            base.Save();
        }

        //возвращает объект класса Friendship(друга) по id пользователей 
        public Friendship SearchByUsers(string idU, string idF)
        {
            return _context.Friendship
                .FirstOrDefault(s => s.User_id == idF && s.Friend_id == idU);
        }


        //получает список друзей
        public IQueryable<Friendship> GetListFriends(string id)
        {
            var list = _context.Friendship
                .Where(s => s.User_id == id);
            return list;
        }



        public Friendship SearchById(int id)
        {
            return _context.Friendship.Find(id);
        }



        //функция проверяет являются ли пользователи друзьями
        public bool Check(string uId, string fId)
        {
            var result = _context.Friendship
                .FirstOrDefault(x => x.User_id == uId && x.Friend_id == fId);
            if (result != null)
                return true;
            return false;
        }

        //получить список id друзей по id-пользователя
        public List<string> GetListFriendsId(string id)
        {
            var list = _context.Friendship
                .Where(s => s.User_id == id).Select(i=>i.Friend_id).ToList();
            return list;
        }

        //извлекает список друзей из списка
        //public IQueryable<Friendship> SortFriendList(IQueryable<Friendship> friends)
        //{
        //    var resultFrindsList =context
        //}
    }
}
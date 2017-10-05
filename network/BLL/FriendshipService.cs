using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class FriendshipService
    {
        NetworkContext db = new NetworkContext();
        private IFriendshipRepository friendRepository;

        private IRequestRepository requestRepository;
        public RepositoryBase reposBase;

        public FriendshipService()
        {
            friendRepository=new FriendshipRepository(db);
            requestRepository=new RequestRepository(db);
            reposBase = new RepositoryBase(db);
        }
        

        //FRIENDSHIPS


        public void AddFriendship(Friendship friend)
        {
            if (friend != null)
            {
                friendRepository.AddFriend(friend);
                friendRepository.Save();
            }
           
        }

        public void DeleteFriendship(int id )
        {
            Friendship friendshipU = friendRepository.SearchById(id);
            Friendship friendshipF = friendRepository.SearchByUsers(friendshipU.User_id, friendshipU.Friend_id);
        
            friendRepository.DeleteFriend(friendshipU.Id);
            friendRepository.Save();
            friendRepository.DeleteFriend(friendshipF.Id);
            friendRepository.Save();

        }

        public IQueryable<Friendship> GetFriendList(string id)
        {
           
            return friendRepository.GetListFriends(id);
        }

        public Friendship SearchFriendship(int id)
        {
            return friendRepository.SearchById(id);
        }

        public Friendship SearchByUsers(string idU, string idF)
        {
            return friendRepository.SearchByUsers(idU, idF);
        }

        //получаем список id всех друзей
        public List<string> GetFriendsIdsList(string id)
        {
            return friendRepository.GetListFriendsId(id);
        }

        



      

        //check friendship
        public bool CheckFriendship(string userId, string friendId)
        {
            if (friendRepository.Check(userId, friendId))
                return true;
            return false;
        }








        //REQUESTS

        public void AddRequest(Requests request)
        {
            try
            {
                requestRepository.AddRequest(request);
                requestRepository.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void CancelRequest(Requests request)
        {
            requestRepository.CancelRequests(request);
            requestRepository.Save();
        }

        public Requests SearchRequest(int id)
        {
            return requestRepository.SearchById(id);
        }


        public IQueryable<Requests> CurrentRequestses(string id)
        {
            return  requestRepository.GetActiveRequests(id);
        }

        public void UpdateRequest(Requests requests)
        {
            requestRepository.Update(requests);
            requestRepository.Save();
        }

        public IQueryable<Requests> RequestList(string id)
        {
            return requestRepository.GetActiveRequests(id);
        }

        public Requests SearchUsers(string userIng, string userEd)
        {
            return requestRepository.SearchByUsersId(userIng, userEd);
        }
        
        //check requests
        public bool Check (string idEd,string idIng)
        {
           
                var request = requestRepository.CheckRequests(idEd, idIng);
                if (request != null)
                 return true;
                    return false;
          
          
        }

        //public void Save()
        //{
        //    requestRepository.Save();
        //}

    }
}
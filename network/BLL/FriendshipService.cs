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

        public FriendshipService()
        {
            friendRepository=new FriendshipRepository(db);
            requestRepository=new RequestRepository(db);
        }

        
        public void AddFriendship(Friendship friend)
        {
            friendRepository.AddFriend(friend);
            friendRepository.Save();
        }

        public void DeleteFriendship(Friendship friend)
        {
            Friendship friendship1 = friendRepository.SearchById(friend.Id);
            
            Friendship friendship2 = friendRepository.SearchBySecondUserId(friendship1.Friend_id);

            friendRepository.DeleteFriend(friendship1);
            friendRepository.Save();
            friendRepository.DeleteFriend(friendship2);
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

        public Friendship SearchByFriend(string id)
        {
            return friendRepository.SearchBySecondUserId(id);
        }

        public void AddRequest(Requests request)
        {
            requestRepository.NewRequest(request);
            requestRepository.Save();
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
            return  requestRepository.SearchRequests(id);
        }

        public void UpdateRequest(Requests requests)
        {
            requestRepository.Update(requests);
            requestRepository.Save();
        }

        public IQueryable<Requests> RequestList(string id)
        {
            return requestRepository.ShowNewRequests(id);
        }

        public Requests SearchUsers(string userIng, string userEd)
        {
            return requestRepository.SearchByUsersId(userIng, userEd);
        }

        public void Save()
        {
            requestRepository.Save();
        }


        public bool Check (string idEd,string idIng)
        {
           
                var request = requestRepository.ReturnRequests(idEd, idIng);
                if (request != null)
                 return true;
                    return false;
          
          
        }

    }
}
using DAL.EF;
using DAL.Enums;
using DAL.IRepository;
using DAL.Repository;
using System;
using System.Linq;

namespace BLL
{
    public class FriendshipService
    {
        private readonly IFriendshipRepository _friendRepository;
        private readonly IRequestRepository _requestRepository;
        
        public FriendshipService(FriendshipRepository friendshipRepository, RequestRepository requestRepository)
        {
            _friendRepository = friendshipRepository;
            _requestRepository = requestRepository;
        }


        //FRIENDSHIPS

        public void AddFriendship(Friendship friend)
        {
            if (friend != null)
            {
                _friendRepository.Add(friend);
            }
           
        }

        public void DeleteFriendship(int id )
        {
            Friendship friendshipU = _friendRepository.SearchById(id);
            Friendship friendshipF = _friendRepository.SearchByUsers(friendshipU.User_id, friendshipU.Friend_id);

            _friendRepository.Delete(friendshipU.Id);
            _friendRepository.Delete(friendshipF.Id);
        }

        public IQueryable<Friendship> GetFriendList(string id)
        {
           
            return _friendRepository.GetListFriends(id);
        }
        
        public Friendship SearchByUsers(string idU, string idF)
        {
            return _friendRepository.SearchByUsers(idU, idF);
        }
              
        //check friendship
        public bool CheckFriendship(string userId, string friendId)
        {
            if (_friendRepository.Check(userId, friendId))
                return true;
            return false;
        }

        


        //REQUESTS

        public void AddRequest(Requests request)
        {
            try
            {
                _requestRepository.Add(request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void CancelRequest(Requests request)
        {
            _requestRepository.Cancel(request);
        }

        public Requests SearchById(int id)
        {
            return _requestRepository.SearchById(id);
        }


        public IQueryable<Requests> CurrentRequestses(string id)
        {
            return _requestRepository.GetActiveRequests(id);
        }

        public void UpdateRequest(Requests requests)
        {
            _requestRepository.Update(requests);
        }

        public IQueryable<Requests> RequestList(string id)
        {
            return _requestRepository.GetActiveRequests(id);
        }

        public Requests SearchByUser(string userIng, string userEd)
        {
            return _requestRepository.SearchByUsersId(userIng, userEd);
        }
        
        //check requests
        public bool Check (string idEd,string idIng)
        {
            var request = _requestRepository.CheckRequests(idEd, idIng);
            if (request != null)
                return true;
                return false;
        }

        //change status of request to  Active
        public void StatusToActive(int id)
        {
            _requestRepository.EditStatusOfRequest(id, FriendshipStatus.Active);
        }
        //change status of request to  Declined
        public void StatusToDeclined(int id)
        {
            _requestRepository.EditStatusOfRequest(id, FriendshipStatus.Declined);
        }
        //change status of request to  Accepted
        public void StatusToAccepted(int id)
        {
            _requestRepository.EditStatusOfRequest(id, FriendshipStatus.Accepted);
        }
        //change status of request to  Ignored
        public void StatusToIgnored(int id)
        {
            _requestRepository.EditStatusOfRequest(id, FriendshipStatus.Ignored);
        }

    }
}
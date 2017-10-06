using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class UserService
    {
        private NetworkContext db = new NetworkContext();

        private IUserRepository userRepository;
        private IImagesRepository imagesRepository;
        private IFamilyStatusRepository familyStatusRepository;
        public RepositoryBase reposBase;

        public UserService()
        {
            userRepository=new UserRepository(db);
            imagesRepository=new ImagesRepository(db);
            familyStatusRepository=new FamilyStatusRepository(db);
            reposBase = new RepositoryBase(db);

        }

        public  IQueryable<UserDetails> GetUser(string id)
        {
            var users = db.UserDetails
                .Where(s => s.AspNetUsers.Id != id);
            return users;
        }

        public void InsertUser(UserDetails user)
        {
            userRepository.AddUser(user);
        }

        public void EditUser(UserDetails user)
        {
            userRepository.Update(user);
            //base.Save();

        }

        public void DeleteUser(UserDetails user)
        {
            UserDetails us = userRepository.GetUserById(user.Id);
            userRepository.DeleteUser(us.Id);
            //userRepository.Save();
        }

        public UserDetails SearchUser(int id)
        {
            return userRepository.GetUserById(id);
           }

      

        public UserDetails SearchByUserId(string i)
        {
            var item = db.UserDetails
                .SingleOrDefault(s => s.UserId == i);
            return item;
        }







        //позволяет преобразовать список string-Id в int-Id
        public List<int> ConvertListId(List<string> strList)
        {
            List<int> listInt = new List<int>();

            foreach (var strId in strList)
            {
                var idInt = CovertId(strId);
                listInt.Add(idInt);
            }
            return listInt;
        }


        //позволяет получить int-ый id  из string-id
        public int CovertId(string id)
        {
            var user = SearchByUserId(id);
            return user.Id;
        }







        //позволяет преобразовать string-Id в int-Id для метода GetFriendsForSearch()
        public Tuple<int, List<int>> ConvertListIds(string id,List<string> strList )
        {
            int intIduser = CovertId(id);
            List<int> intListFriends = ConvertListId(strList);
            return Tuple.Create<int, List<int>>(intIduser, intListFriends);
        }


        //позволяет вернуть все данные для друзей по списку id
        public List<UserDetails> GetUserDetailsByListId(List<int> listId)
        {
            List<UserDetails> usersList = new List<UserDetails>();

            foreach (var users in listId)
            {
                var item = SearchUser(users);
                usersList.Add(item);
            }
            return usersList;
        }



        //позволяет исключить список id из списка id
        public List<int> ExcludeListIdInListId(List<int> fullList, List<int> excludingList)
        {
            var diff = fullList.Where(x => !excludingList.Contains(x));
            var result = fullList.Where(x => diff.Contains(x)).ToList();
            return result;
        }
        

        //позволяет получить данные для объектов, полученные путем исключения одного списка из другого
        public List<UserDetails> GetDataForSearch(List<int> listIdsAll, List<int> listIdsFromConvers)
        {

            var list = ExcludeListIdInListId(listIdsAll, listIdsFromConvers);

            return GetUserDetailsByListId(list).ToList();
        }



        

















        // позволяет вернуть все данные для друзей 
        //!!!!!!!!!!!!!!!!!!!!!!!!!
        public List<UserDetails> GetUsersByFriendship(IQueryable<Friendship> friendships)   
        {
            List<UserDetails> usersList=new List<UserDetails>();

            foreach (var users in friendships)
            {
                var item = SearchByUserId(users.Friend_id);
                usersList.Add(item);
            }
            return usersList;
        }
        
        //позволяет получить список прочих пользователей (не друзей)
        //!!!!!!!!!!!!!!!!!!!!!!!!!
        public List<UserDetails> AnotherUsers(IQueryable<Friendship> friendships, List<UserDetails> users)
        {
            List<UserDetails> list=new List<UserDetails>();

            List<UserDetails> friendList=GetUsersByFriendship(friendships);

            var userIds = users.Select(u => u.Id);
            var friendsIds = friendList.Select(f => f.Id);

            var diff = userIds.Where(x => !friendsIds.Contains(x));

            var result = users.Where(x => diff.Contains(x.Id)).ToList();

            return result;
        }
        
        //public IQueryable<FamilyStatus> GetAllFamStatuses()
        //{
        //    return familyStatusRepository.GetListFamStatus();
        //}

        //позволяет получить список int-Id по string-Id 

    }
}








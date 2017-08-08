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

        public UserService()
        {
            userRepository=new UserRepository(db);
            imagesRepository=new ImagesRepository(db);
            familyStatusRepository=new FamilyStatusRepository(db);
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
            userRepository.Save();
        }

        public void EditUser(UserDetails user)
        {
            userRepository.Update(user);
            userRepository.Save();

        }

        public void DeleteUser(UserDetails user)
        {
            UserDetails us = userRepository.GetUserById(user.Id);
            userRepository.DeleteUser(us.Id);
            userRepository.Save();
        }

        public UserDetails SearchUser(int id)
        {
            return userRepository.GetUserById(id);
           }

        public byte[] ReturnImage(int id)
        {
            byte[] imageData = imagesRepository.GetImageById(id).Data;
            return imageData;
        }

        public UserDetails SearchByUserId(string i)
        {
            var item = db.UserDetails
                .SingleOrDefault(s => s.UserId == i);
            return item;
        }

        public IQueryable<FamilyStatus> GetAllFamStatuses()
        {
            return familyStatusRepository.GetListFamStatus();
        }






        // позволяет вернуть список друзей
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
        public List<UserDetails> AnotherUsers(IQueryable<Friendship> friendships, List<UserDetails> users)
        {
            List<UserDetails> list=new List<UserDetails>();

            List<UserDetails> friendList=GetUsersByFriendship(friendships);


            foreach (var user in users)
            {
                if (friendList.Count != 0)
                {
                    foreach (var friend in friendList)
                    {
                        if (friend != user)
                            list.Add(user);
                    }
                }
                else list.Add(user);
            }
            return list;
        }

    }
}
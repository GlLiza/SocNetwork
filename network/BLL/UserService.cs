using System.Linq;
using Microsoft.VisualBasic.ApplicationServices;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;
using Microsoft.AspNet.Identity;

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

    }
}
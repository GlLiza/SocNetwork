using System.Linq;
using network.DAL.IRepository;
using network.DAL.Repository;
using network.BLL.EF;

namespace network.BLL
{
    public class UserService
    {
        NetworkEntities db=new NetworkEntities();

        private IUserRepository userRepository;
        private IImagesRepository imagesRepository;
        private IFamilyStatusRepository familyStatusRepository;

        public UserService()
        {
            userRepository=new UserRepository(db);
            imagesRepository=new ImagesRepository(db);
            familyStatusRepository=new FamilyStatusRepository(db);
        }

        public IQueryable<Users> GetUser()
        {
            var users = db.Users;
            return users;
        }

        public void InsertUser(Users user)
        {
            userRepository.AddUser(user);
            userRepository.Save();
        }

        public void EditUser(Users user)
        {
            userRepository.Update(user);
            userRepository.Save();

        }

        public void DeleteUser(Users user)
        {
            Users us = userRepository.GetUserById(user.Id);
            userRepository.DeleteUser(us.Id);
            userRepository.Save();
        }

        public Users SearchUser(int id)
        {
            Users user = userRepository.GetUserById(id);
            return user;
        }

        public byte[] ReturnImage(int id)
        {
            byte[] imageData = imagesRepository.GetImageById(id).Data;
            return imageData;
        }





    }
}
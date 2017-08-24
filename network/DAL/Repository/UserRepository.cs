using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class UserRepository : RepositoryBase,IUserRepository
    {
        public UserRepository(NetworkContext cont)
        {
            context = cont;
        }

        public void AddUser(UserDetails user)
        {
           context.UserDetails.Add(user);
        }

        public void DeleteUser(int userId)
        {
            UserDetails user = context.UserDetails.Find(userId);
            context.UserDetails.Remove(user);
        }
        
        public IEnumerable<UserDetails> GetUserList()
        {
            return context.UserDetails;
        }
        
        public void Update(UserDetails user)
        {
            var us = context.UserDetails.Find(user.Id);
            context.Entry(us).CurrentValues.SetValues(user);
        }
        
        public UserDetails GetUserById(int id)
        {
            var item = context.UserDetails.Find(id);
            return item;
        }

        IEnumerable<UserDetails> Users()
        {
            foreach (var a in GetUserList())
            {
                yield return a;
            }
        }
    }
}


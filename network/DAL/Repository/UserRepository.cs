using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using network.BLL;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private NetworkEntities context;

        public UserRepository(NetworkEntities cont)
        {
            context = cont;
        }

        public void AddUser(Users user)
        {
           context.Users.Add(user);
        }

        public void DeleteUser(int userId)
        {
            Users user = context.Users.Find(userId);
            context.Users.Remove(user);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposed)
        {
            if (!this.disposed)
            {
                if (disposed)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Users> GetUserList()
        {
            return context.Users;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Users user)
        {
            context.Entry(user).State = EntityState.Modified;
        }

        public Users GetUserById(int id)
        {
            var item = context.Users.Find(id);
            return item;
        }

        IEnumerable<Users> Users()
        {
            foreach (var a in GetUserList())
            {
                yield return a;
            }
        }
    }
}


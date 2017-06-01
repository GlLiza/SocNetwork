using System;
using System.Collections.Generic;
using System.Data.Entity;
using network.BLL.EF;
using network.DAL.IRepository;

namespace network.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private NetworkEntities1 context;

        public UserRepository(NetworkEntities1 cont)
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

        public IEnumerable<UserDetails> GetUserList()
        {
            return context.UserDetails;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(UserDetails user)
        {
            context.Entry(user).State = EntityState.Modified;
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


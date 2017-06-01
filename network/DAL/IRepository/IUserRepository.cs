using System;
using System.Collections.Generic;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IUserRepository:IDisposable
    {
        IEnumerable<UserDetails> GetUserList();
        void AddUser (UserDetails user);
        void DeleteUser (int userId);
        void Update(UserDetails user);
        void Save();

        UserDetails GetUserById(int id);
    }
}

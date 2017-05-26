using System;
using System.Collections.Generic;
using network.BLL.EF;

namespace network.DAL.IRepository
{
    interface IUserRepository:IDisposable
    {
        IEnumerable<Users> GetUserList();
        void AddUser (Users user);
        void DeleteUser (int userId);
        void Update(Users user);
        void Save();

        Users GetUserById(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using network.BLL.EF;
using network.DAL.Enums;

namespace network.DAL.IRepository
{
    interface IUserRepository:IDisposable
    {
        IEnumerable<UserDetails> GetUserList();
        void AddUser (UserDetails user);
        void DeleteUser (int userId);
        void Update(UserDetails user);
        int ReturnIntId(string id);
        UserDetails GetUserById(int? id);
        IQueryable<UserDetails> GetListFemal(Gender gender);
        IQueryable<UserDetails> GetListMal(Gender gender);
    }
}

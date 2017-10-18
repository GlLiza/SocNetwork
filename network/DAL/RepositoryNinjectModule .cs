using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using network.BLL.EF;
using network.DAL.IRepository;
using Ninject.Modules;

namespace network.DAL
{
    public class RepositoryNinjectModule:NinjectModule
    {
        public override void Load()
        {
            this.Bind<IMessageRepository>().To<Messages>();
        }
    }
}
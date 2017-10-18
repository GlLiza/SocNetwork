using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using network.BLL.EF;

namespace network.DAL.Repository
{
    public class TestRepository : RepositoryBase
    {
        public TestRepository(NetworkContext con) : base(con)
        {
        }
    }
}
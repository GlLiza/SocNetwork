using DAL.EF;

namespace DAL.Repository
{
    public class TestRepository : RepositoryBase
    {
        public TestRepository(NetworkContext con) : base(con)
        {
        }
    }
}
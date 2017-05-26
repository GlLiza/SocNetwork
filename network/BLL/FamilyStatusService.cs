using System.Linq;
using network.DAL.IRepository;
using network.DAL.Repository;
using network.BLL.EF;

namespace network.BLL
{
    public class FamilyStatusService
    {
        NetworkEntities db = new NetworkEntities();

        private IFamilyStatusRepository familyStatusRepository;

        public FamilyStatusService()
        {
            familyStatusRepository=new FamilyStatusRepository(db);
        }

        public IQueryable<FamilyStatus> GetFamStat()
        {
            var famStat = from s in db.FamilyStatus
                         select s;
            return famStat;
        }

        public void InsertFamStat (FamilyStatus famStatus)
        {
            familyStatusRepository.AddFamStatus(famStatus);
            familyStatusRepository.Save();
        }

        public void EditUser(FamilyStatus famStatus)
        {
            familyStatusRepository.UpdateFamStatus(famStatus);
            familyStatusRepository.Save();

        }

        public void DeleteFamStat (FamilyStatus famStatus)
        {
            FamilyStatus famStat = familyStatusRepository.GetStatById(famStatus.Id);
            familyStatusRepository.DeleteFamStatus(famStatus.Id);
            familyStatusRepository.Save();
        }
    }
}
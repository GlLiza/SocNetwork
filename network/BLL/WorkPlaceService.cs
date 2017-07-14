using System.Collections.Generic;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL.EF
{
    public class WorkPlaceService
    {
        NetworkContext db = new NetworkContext();
        private IWorkPlaceRepository workPlaceRepository;

        public WorkPlaceService()
        {
            workPlaceRepository=new WorkPlaceRepository(db);
        }

        public void AddWorkPlace(WorkPlace place)
        {
            workPlaceRepository.AddWorkPlace(place);
            workPlaceRepository.Save();
        }

        public void DeleteWorkPlace(WorkPlace place)
        {
            WorkPlace pl = workPlaceRepository.GetPlaseById(place.Id);
            workPlaceRepository.DeleteWorkPlace(pl.Id);
            workPlaceRepository.Save();
        }

        public WorkPlace SearchPlace(int id)
        {
          return workPlaceRepository.GetPlaseById(id);
        }

        public void Update(WorkPlace place)
        {
            workPlaceRepository.Update(place);
            workPlaceRepository.Save();
        }

        public IEnumerable<WorkPlace> GetListWorks(int useId)
        {
            return workPlaceRepository.GetListWorks(useId);
        }



    }
}
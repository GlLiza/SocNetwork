using System.Collections.Generic;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;

namespace network.BLL
{
    public class WorkPlaceService
    {

        private readonly IWorkPlaceRepository _workPlaceRepository;

        //public WorkPlaceService()
        //{
        //}

        public WorkPlaceService(WorkPlaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        public void AddWorkPlace(WorkPlace place)
        {
            _workPlaceRepository.AddWorkPlace(place);
        }

        public void DeleteWorkPlace(WorkPlace place)
        {
            WorkPlace pl = _workPlaceRepository.GetPlaseById(place.Id);
            _workPlaceRepository.DeleteWorkPlace(pl.Id);
        }

        public WorkPlace SearchPlace(int id)
        {
          return _workPlaceRepository.GetPlaseById(id);
        }

        public void Update(WorkPlace place)
        {
            _workPlaceRepository.Update(place);
        }

        public IEnumerable<WorkPlace> GetListWorks(int useId)
        {
            return _workPlaceRepository.GetListWorks(useId);
        }

    }
}
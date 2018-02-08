using System.Collections.Generic;
using DAL.Repository;
using DAL.EF;
using DAL.IRepository;

namespace BLL
{
    public class WorkPlaceService
    {

        private readonly IWorkPlaceRepository _workPlaceRepository;

        public WorkPlaceService(WorkPlaceRepository workPlaceRepository)
        {
            _workPlaceRepository = workPlaceRepository;
        }

        public void AddWorkPlace(WorkPlace place)
        {
            _workPlaceRepository.Add(place);
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
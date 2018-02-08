using Microsoft.AspNet.Identity;
using DAL.EF;
using System.Linq;
using System.Web.Mvc;
using BLL.ViewModels;
using BLL;

namespace network.Controllers
{
    public class SettingsController : Controller
    {
        private readonly UserService _userService;
        private readonly WorkPlaceService _workPlaceService;
        private readonly SchoolService _schoolService;
        private readonly LocationService _locService;
        private readonly ImageService _imgService;

        public SettingsController(UserService userService, WorkPlaceService workPlaceService, SchoolService schoolService,
            LocationService locService, ImageService imgService)
        {
            _userService = userService;
            _workPlaceService = workPlaceService;
            _schoolService = schoolService;
            _locService = locService;
            _imgService = imgService;
        }

        // GET: Settings
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult EditPersonalInfo()
        {
            var idUser =_userService.ConvertId(User.Identity.GetUserId()) ;
            var persData = _userService.SearchUser(idUser);
            var img = _imgService.SearchImg(persData.ImagesId);

            PersonalInfoViewModel model = new PersonalInfoViewModel()
            {
                Id = idUser,
                Gender=persData.Gender,
                ImageId = persData.ImagesId,
                Name = persData.Name,
                Firstname = persData.Firstname,
                DateOfBirthday = persData.DateOfBirthday,
                SelectedStatus = persData.FamilyStatusId,
                FamilyStatus = _userService.GetFamStatuses()
            };

            return PartialView("_GetPersonalInfo",model);
        }

        public ActionResult EditPersInfo(PersonalInfoViewModel model)
        {
            var us = _userService.SearchUser(model.Id);
            UserDetails user = new UserDetails()
            {
                Id=model.Id,
                UserId=us.UserId,
                Gender=model.Gender,
                FamilyStatusId=model.SelectedStatus,
                Name=model.Name,
                Firstname=model.Firstname,
                ImagesId=model.ImageId,
                DateOfBirthday=model.DateOfBirthday,
                HomeTownLocationId=us.HomeTownLocationId,
                CurrentLocationId=us.CurrentLocationId
            };
            _userService.EditUser(user);
            
            return RedirectToAction("Index");
        }

        public ActionResult AddSchool()
        {
            School sch = new School();
            return PartialView("_AddSchool",sch);
        }

        public ActionResult AddNewSchool(School school)
        {
            var userId = _userService.GetIntUserId(User.Identity.GetUserId());
            school.UserId = userId;
            _schoolService.AddSchool(school);
           
            return RedirectToAction("Index","Settings");
        }

        public ActionResult EditSchool()
        {
            var userId = _userService.GetIntUserId(User.Identity.GetUserId());
            var schoolsList = _schoolService.GetListSchoolsForUser(userId).ToList();
            EditSchoolInfo model = new EditSchoolInfo()
            {
                Schools = schoolsList
            };

            return PartialView("_EditSchool", model);
        }

        public ActionResult EditSchoolInfo(EditSchoolInfo model)
        {
            foreach (var item in model.Schools)
            {
                _schoolService.UpdateSchool(item);
            }
            return RedirectToAction("Index", "Settings");
        }

        [HttpGet]
        public ActionResult AddPlaceWork()
        {
            return PartialView("_AddPlaceWork");
        }

        [HttpPost]
        public ActionResult AddPlaceWork(WorkPlace work)
        {
            var userId = _userService.GetIntUserId(User.Identity.GetUserId());
            work.UserId = userId;
            _workPlaceService.AddWorkPlace(work);
            return RedirectToAction("Index");
        }

       
        public ActionResult EditWorksPlace()
        {
            var userId = _userService.GetIntUserId(User.Identity.GetUserId());
            var workPlList = _workPlaceService.GetListWorks(userId).ToList();
            //EditWorksPlace model = new EditWorksPlace()
            //{
            //    WorkPlaces = workPlList
            //};
            return PartialView("_EditWorksPlace", workPlList);
        }

        public ActionResult EditWorksPlaceInfo(WorkPlace model)
        {
            //foreach (var item in model)
            //{
                _workPlaceService.Update(model);
            //}

            return RedirectToAction("Index");
        }

    }
}

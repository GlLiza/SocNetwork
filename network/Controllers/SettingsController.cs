using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.Views.ViewModels;
using System.Web.Mvc;

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
                WorkPlaceId=us.WorkPlaceId,
                SchoolId=us.SchoolId,
                HomeTownLocationId=us.HomeTownLocationId,
                CurrentLocationId=us.CurrentLocationId
            };
            _userService.EditUser(user);
            

            return RedirectToAction("Index");
        }





    }
}

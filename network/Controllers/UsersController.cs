using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.Views.ViewModels;

namespace network.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly WorkPlaceService _workPlaceService;
        private readonly SchoolService _schoolService;
        private readonly LocationService _locService;
        private readonly FriendshipService _friendServ;
        private readonly ImageService _imgService;

        public UsersController(UserService userService, WorkPlaceService workPlaceService, SchoolService schoolService,
            LocationService locService, FriendshipService friendServ, ImageService imgService)
        {
            _userService = userService;
            _workPlaceService = workPlaceService;
            _friendServ = friendServ;
            _schoolService = schoolService;
            _imgService = imgService;
            _locService = locService;
        }


    [Authorize]
    [HttpGet]
    public ActionResult Index()
    {
        PageViewModel model = new PageViewModel();
        var a = User.Identity.GetUserId();
        var currentUser = _userService.SearchByUserId(a);

        if (currentUser != null)
    {
        var user = currentUser;

        model.Id = user.Id;
        model.Name = user.Name;
        model.Firstname = user.Firstname;
        model.DateOfBirthday = user.DateOfBirthday;
        model.FamStat = user.FamilyStatus;


        if (user.Images != null)
        {
            model.Image = user.Images.Data;
        }

        var curLoc = _locService.GetLocation(user.CurrentLocationId);
        model.CurrentLocation = curLoc;

            return View(model);
        }

        return RedirectToAction("Login", "Account");
    }

    public ActionResult UsersPage(int id)
    {
        var friend = _userService.SearchUser(id);

        if (_friendServ.CheckFriendship(User.Identity.GetUserId(), friend.UserId))
        {
            FriendShow modell = new FriendShow();
            modell.Id = friend.Id;
            modell.Name = friend.Name;
            modell.Firstname = friend.Firstname;
            modell.DateOfBirthday = friend.DateOfBirthday;
            modell.FamStat = friend.FamilyStatus;
            modell.Image = friend.Images.Data;

            Location curLoc = _locService.GetLocation(friend.Id);

            modell.CurrentLocation = curLoc;


            return View("FriendsPage", modell);
        }
        else
        {
            NotFriendShow model = new NotFriendShow();
            model.Id = friend.Id;
            model.Name = friend.Name;
            model.Firstname = friend.Firstname;
            model.Image = friend.Images.Data;

            return View("NotFriendsPage", model);
        }
    }


    [HttpGet]
    public ActionResult BrowseUsers()
    {
        List<ShowUserViewModel> model = new List<ShowUserViewModel>();


        var friendList = _friendServ.GetFriendList(User.Identity.GetUserId());
        var users = _userService.GetUser().ToList();
                    
        int currentUserId = _userService.ConvertId(User.Identity.GetUserId());

        var othersUsers = _userService.AnotherUsers(friendList, users);

        foreach (var u in othersUsers)
        {
            if (u.Id != currentUserId)
            {
                ShowUserViewModel item = new ShowUserViewModel();

                item.Id = u.Id;
                item.Firstname = u.Firstname;
                item.Name = u.Name;
                item.Image = u.Images.Data;
                item.User = u.AspNetUsers;
                item.User = u.AspNetUsers;
                   model.Add(item);
            }

            }
            ViewData["Error"] = TempData["message"];

            return View(model);
    }
        
    public ActionResult CreateProfile(int? id)
    {
        UserDetails user = _userService.SearchUser(id);
            var model = new CreateUserViewModel
            {
                Id = user.Id,
                FamilyStatus = _userService.GetFamStatuses(),
                ListOfCountry = _userService.GetCountries(),
                MonthList=_userService.GetMonth()
            };
        return View("CreateProfile", model);
    }

    // POST: Users/Edit/5
    [HttpPost]
    public ActionResult CreateProfile(CreateUserViewModel model)
    {
        try
        {
            Location currentLoc = new Location();
            Location homeLocation = new Location();
            WorkPlace work = new WorkPlace();
            School school = new School();
            Images headerImage = new Images();
            UserDetails user = _userService.SearchUser(model.Id);

            if (model.Image != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(model.Image.InputStream))
                {
                    imageData = binaryReader.ReadBytes(model.Image.ContentLength);
                }
                headerImage.Name = model.Image.FileName;
                headerImage.Data = imageData;
                headerImage.ContentType = model.Image.ContentType;
                    headerImage.Date = DateTime.Now;

                _imgService.InsertImage(headerImage);

                user.ImagesId = headerImage.Id;
                _userService.EditUser(user);
            }

            currentLoc.Country = model.Country;

            _locService.AddLocation(currentLoc);
            user.CurrentLocationId = currentLoc.Id;

            homeLocation.Country = model.HomeCountry;

            _locService.AddLocation(homeLocation);
            user.HomeTownLocationId = homeLocation.Id;


            school.Name = model.SchoolName;
            school.GraduationYear = model.GraduationYear;

            _schoolService.AddSchool(school);
            user.SchoolId = school.Id;

            work.CompanyName = model.CompanyName;
            work.Position = model.Position;
            work.Description = model.Description;
            work.StartMonth = model.WorkPeriod.StartMonth;
            work.StartYear = model.WorkPeriod.StartYear;
            work.EndMonth = model.WorkPeriod.EndMonth;
            work.EndYear = model.WorkPeriod.EndYear;   
                                 
                
            _workPlaceService.AddWorkPlace(work);
            user.WorkPlaceId = work.Id;


            user.Name = model.Name;
            user.Firstname = model.Firstname;

            user.DateOfBirthday = model.DateOfBirthday;
            user.FamilyStatusId = model.SelectedStatus;
            user.Gender = Convert.ToString(model.Gender);

            _userService.EditUser(user);


            return RedirectToAction("Index", "Users");
        }

        catch (Exception e)
        {
            return View();
        }
    }

    // GET
    public ActionResult ChangePhoto()
    {
        ChangePhotoViewModel model = new ChangePhotoViewModel();
        model.Id = GetId();
        return PartialView("_ChangePhoto", model);
    }

    [HttpPost]
    public ActionResult ChangePhoto(ChangePhotoViewModel viewModel)
    {
        var imageFile = viewModel.Image;
        if (imageFile != null)
        {
            Images headerImage = new Images();
            var user = _userService.SearchUser(GetId());


            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(imageFile.InputStream))
            {
                imageData = binaryReader.ReadBytes(imageFile.ContentLength);
            }
            headerImage.Name = imageFile.FileName;
            headerImage.Data = imageData;
            headerImage.ContentType = imageFile.ContentType;

            _imgService.InsertImage(headerImage);

            user.ImagesId = headerImage.Id;
            _userService.EditUser(user);

            return RedirectToAction("Index", "Users");
        }

        return HttpNotFound();

    }

    public string GetCities(string CountryName)
    {
        GetCitiesByCountryServiceRef.GlobalWeatherSoapClient obj =new GetCitiesByCountryServiceRef.GlobalWeatherSoapClient {};
        return obj.GetCitiesByCountry(CountryName);
    }   

    public UserDetails GetUser()
    {
        var user = _userService.SearchByUserId(User.Identity.GetUserId());
        return user;
    }

    public int GetId()
    {
        var user = _userService.SearchByUserId(User.Identity.GetUserId());
        return user.Id;
    }            

    //function for convert HttpPostedFileBase to byte[]

    public byte[] ConvertPhoto(HttpPostedFileBase img)
    {
        byte[] imageData = null;

        if (img != null)
        {
            using (var binaryReader = new BinaryReader(img.InputStream))
            {
                imageData = binaryReader.ReadBytes(img.ContentLength);
            }
        }
        return imageData;

    }

    }
}
    



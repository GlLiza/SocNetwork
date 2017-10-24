﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.Views.ViewModels;
using System.Globalization;

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

        public string country = "Belarus";

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
        //var companys = _workPlaceService.GetListWorks(user.Id);
        //var schools = _schoolService.GetListSchools(user.Id);


        model.Id = user.Id;
        model.Name = user.Name;
        model.Firstname = user.Firstname;
        model.DateOfBirthday = user.DateOfBirthday;
        model.FamStat = user.FamilyStatus;


        if (user.Images != null)
        {
            model.Image = user.Images.Data;

        }
        else
        {
            model.Image = DefaultPhoto();
        }

        var curLoc = _locService.GetLocation(user.CurrentLocationId);
        model.CurrentLocation = curLoc;



        //if (homeLocation.Count() > 1)
        //    model.ListHomLoc = homeLocation;
        //else
        //{
        //    foreach (var a in homeLocation)
        //    {
        //        model.HomeLocation = a;

        //    }

        //}


        //if (curLocation.Count() > 1)
        //    model.ListCurLoc = curLocation;
        //else
        //{
        //    foreach (var a in curLocation)
        //    {
        //        model.CurrentLocation = a;
        //    }
        //}



        //if (schools.Count() > 1)
        //    model.ListSchools = schools;
        //else
        //{
        //    foreach (var a in schools)
        //    {
        //        model.School = a;
        //    }
        //}



        //if (companys.Count() > 1)
        //    model.ListPlace = companys;
        //else
        //{
        //    foreach (var a in companys)
        //    {
        //        model.Company = a;
        //    }
        //}


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
        var users = _userService.GetUser(User.Identity.GetUserId()).ToList();

        var othersUsers = _userService.AnotherUsers(friendList, users);

        foreach (var u in othersUsers)
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
        ViewData["Error"] = TempData["message"];

        return View(model);
    }
        
    public ActionResult CreateProfile(int? id)
    {
        UserDetails user = _userService.SearchUser(id);

            //ViewData["Month"] = new SelectList(Enumerable.Range(1, 12).Select(x =>
            //       new SelectListItem()
            //       {
            //           //Text = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[x - 1],
            //           Text = CultureInfo.CurrentCulture.LCID.ToString(),
            //           Value = x.ToString()
            //       }),"Value","Text");



            var model = new CreateUserViewModel
            {
                Id = user.Id,
                FamilyStatus = _userService.GetFamStatuses(),
                ListOfCountry = _userService.GetCountries(),
                MonthList=_userService.GetMonth()
                //City=GetCities(country)
                //ListOfCity = GetCities('Belarus');
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
            else user.Images.Data = DefaultPhoto();

            //currentLoc.City = model.City;
            //currentLoc.State = model.State;
            //currentLoc.Street = model.Street;
            currentLoc.Country = model.Country;

            _locService.AddLocation(currentLoc);
            user.CurrentLocationId = currentLoc.Id;

            homeLocation.Country = model.HomeCountry;
            //homeLocation.City = model.HomeCity;
            //homeLocation.State = model.HomeState;
            //homeLocation.Street = model.HomeStreet;

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


    public byte[] DefaultPhoto()
    {
        var photo = _imgService.SearchImg(1058);

        return photo.Data;
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
    



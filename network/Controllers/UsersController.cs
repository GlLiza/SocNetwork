    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualBasic.ApplicationServices;
    using network.BLL;
    using network.BLL.EF;
    using network.DAL.Repository;
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

            var model = new CreateUserViewModel
            {
                Id = user.Id,
                FamilyStatus = _userService.GetFamStatuses()
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

                    _imgService.InsertImage(headerImage);

                    user.ImagesId = headerImage.Id;
                    _userService.EditUser(user);
                }
                else user.Images.Data = DefaultPhoto();

                currentLoc.City = model.City;
                currentLoc.State = model.State;
                currentLoc.Street = model.Street;
                currentLoc.Country = model.Country;

                _locService.AddLocation(currentLoc);
                user.CurrentLocationId = currentLoc.Id;

                homeLocation.Country = model.HomeCountry;
                homeLocation.City = model.HomeCity;
                homeLocation.State = model.HomeState;
                homeLocation.Street = model.HomeStreet;

                _locService.AddLocation(homeLocation);
                user.HomeTownLocationId = homeLocation.Id;


                school.Name = model.SchoolName;
                school.GraduationYear = model.GraduationYear;

                _schoolService.AddSchool(school);
                user.SchoolId = school.Id;

                work.CompanyName = model.CompanyName;
                work.Position = model.Position;
                work.Description = model.Description;
                work.StartDate = model.StartDate;
                work.EndDate = model.EndDate;

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






        // GET: Users/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    var user = _userService.SearchUser(id);
        //    return View("Delete",user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost]
        //public ActionResult Delete(UserDetails u)
        //{
        //    try
        //    {
        //        var user = _userService.SearchUser(u.Id);
        //        var USER = db.AspNetUsers.Find(user.UserId);


        //        //if (u.ImagesId != null)
        //        //{
        //        //    var photo = imgServ.SearchImg(u.ImagesId);
        //        //    imgServ.DeleteImage(photo);
        //        //}

        //        _userService.DeleteUser(user);


        //        if (USER.Id!=null)
        //        db.AspNetUsers.Remove(USER);

        //        db.SaveChanges();

        //        return RedirectToAction("LogOff", "Account");
        //    }
        //    catch (Exception e)
        //    {
        //        return View();
        //    }
        //}

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





        // GET: Users/Create
        //public ActionResult CreatePhotoalbum(int id)
        //{
        //    UserDetails user = userService.SearchUser(id);


        //    return View("Create");
        //}


        //public ActionResult Add(HttpPostedFileBase img)
        //{
        //    var profileImage = new Image();

        //    if (img != null)
        //    {
        //        using (Image img = Image.FromStream(img.InputStream)) ;
        //        {
        //            byte[] file = new byte[img.InputStream.Length];
        //            BinaryReader reader = new BinaryReader(img.InputStream);
        //            img.InputStream.Seek(0, SeekOrigin.Begin);

        //            file = reader.ReadBytes((int)img.InputStream.Length);
        //        }
        //    }
        //}

        //public ActionResult AddPhoto(HttpPostedFileBase file)
        //{
        //    var profileImage = new Image();

        //    if (file != null)
        //    if (file != null)
        //    {
        //        using (Image img = Image.FromStream(file.InputStream)) ;
        //        {
        //            byte[] data = new byte[file.InputStream.Length];
        //            BinaryReader reader = new BinaryReader(file.InputStream);
        //            file.InputStream.Seek(0, SeekOrigin.Begin);

        //            data = reader.ReadBytes((int)file.InputStream.Length);
        //        }
        //    }
        //}



        // POST: Users/Create
        //[HttpPost]
        //public ActionResult CreatePhotoalbum(HttpPostedFileBase img, UserDetails user)
        //{
        //    try
        //    { 


        //{
        //    userService.InsertUser(user);

        //    if (img != null)
        //    {
        //        byte[] imageData = null;
        //        using (var binaryReader = new BinaryReader(img.InputStream))
        //        {
        //            imageData = binaryReader.ReadBytes(img.ContentLength);
        //        }
        //        Images headerImage = new Images()
        //        {

        //            Name = img.FileName,
        //            Data = imageData,
        //            ContentType = img.ContentType
        //        };
        //        imgRepository.AddImage(headerImage);

        //        imgRepository.Save();
        //        UserDetails item = userService.SearchUser(user.Id);
        //        item.ImagesId = headerImage.Id;
        //        userService.EditUser(user);


        //    }

        //       return RedirectToAction("Index");
        //    }
        //    catch (Exception c)
        //    {
        //        return View();
        //    }
        //}

        //public ActionResult GetImage(int id)
        //{

        //    byte[] imageData = userService.ReturnImage(id);
        //    var img = new FileStreamResult(new System.IO.MemoryStream(imageData), "image/jpeg");
        //    return img;
        //}



        //// GET: Users/Create
        //public ActionResult Create()
        //{

        //    return View("Create");
        //}

        //// POST: Users/Create
        //[HttpPost]
        //public ActionResult Create(HttpPostedFileBase  img, UserDetails user)
        //{
        //    try
        //    {
        //        userService.InsertUser(user);

        //        if (img!=null)
        //        {
        //                byte[] imageData = null;
        //                using (var binaryReader = new BinaryReader(img.InputStream))
        //                {
        //                    imageData = binaryReader.ReadBytes(img.ContentLength);
        //                }
        //            Images headerImage = new Images()
        //                {

        //                    Name = img.FileName,
        //                    Data = imageData,
        //                    ContentType = img.ContentType
        //                };
        //            imgRepository.AddImage(headerImage);

        //            imgRepository.Save();
        //            UserDetails item = userService.SearchUser(user.Id);
        //            item.ImagesId = headerImage.Id;
        //            userService.EditUser(user);


        //        }

        //            return RedirectToAction("Index");
        //    }
        //    catch (Exception c)
        //    {
        //        return View();
        //    }
        //}



        //[HttpGet]
        //public ActionResult ShowUsers()
        //{
        //    string id = User.Identity.GetUserId();
        //    var users = userService.GetUser(id).ToList();

        //    List<ShowUserViewModel> model = new List<ShowUserViewModel>();

        //    foreach (var b in users)
        //    {
        //        ShowUserViewModel userr = new ShowUserViewModel();

        //        userr.Id = b.Id;
        //        var user = userService.SearchUser(userr.Id);

        //        userr.Firstname = user.Firstname;
        //        userr.Name = user.Name;
        //        //user.Image = b.Images.Data;
        //        //user.User = b.AspNetUsers;

        //        //var friend = friendServ.SearchByFriend(b.UserId);

        //        //if (friend != null)
        //        //    //user.User = b.AspNetUsers;
        //        model.Add(userr);
        //    }

        //    return View(model);

        //    }


    }
}
    



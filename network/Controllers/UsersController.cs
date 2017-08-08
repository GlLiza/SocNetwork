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

        public NetworkContext db = new NetworkContext();
        public UserService userService;
        public WorkPlaceService workPlaceService;
        public SchoolService schoolService;
        public LocationService locService;
        public FriendshipService friendServ;
        public ImageService imgService;

        public UsersController()
        {
            userService = new UserService();
            locService=new LocationService();
            workPlaceService=new WorkPlaceService();
            friendServ =new FriendshipService();
            schoolService=new SchoolService();
            imgService=new ImageService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            PageViewModel model = new PageViewModel();
            var currentUser = GetUser();

            if (currentUser!=null)
            {

                var user = currentUser;
                // var User = userService.SearchByUserId(user.UserId);             //from table AspNetUsers
                var companys = workPlaceService.GetListWorks(user.Id);
                var schools = schoolService.GetListSchools(user.Id);
                //var homeLocation=locService.ListHomeLoc(user.HomeTownLocationId);
                //var curLocation = locService.ListCurrentLoc(user.CurrentLocationId);




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

                var curLoc = locService.GetLocation(user.CurrentLocationId);
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




        [HttpGet]
        public ActionResult BrowseUsers()
        {
            List<ShowUserViewModel> model = new List<ShowUserViewModel>();


            var friendList = friendServ.GetFriendList(User.Identity.GetUserId());
            var users = userService.GetUser(User.Identity.GetUserId()).ToList();

            var othersUsers = userService.AnotherUsers(friendList, users);

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

            return View(model);
        }

      


       




        public ActionResult CreateProfile(int id)
        {
            UserDetails user = userService.SearchUser(id);


            
            var model = new CreateUserViewModel
            {
                Id=user.Id,
                //UserId = user.UserId,
                FamilyStatus =userService.GetAllFamStatuses(),
               // GenderStatus = userService.GetAllGenders()

            };
            return View("CreateProfile", model);
        }
        
        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult CreateProfile(CreateUserViewModel model)
      {
            try
            {
                Location currentLoc=new Location();
                Location homeLocation=new Location();
                WorkPlace work=new WorkPlace();
                School school=new School();
                Images headerImage = new Images();

                UserDetails user = userService.SearchUser(model.Id);



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

                    imgService.InsertImage(headerImage);

                    user.ImagesId = headerImage.Id;
                    userService.EditUser(user);
                }
                else user.Images.Data = DefaultPhoto();
                


                currentLoc.City = model.City;
                currentLoc.State = model.State;
                currentLoc.Street = model.Street;
                currentLoc.Country = model.Country;

                locService.AddLocation(currentLoc);

                user.CurrentLocationId = currentLoc.Id;




                homeLocation.Country = model.HomeCountry;
                homeLocation.City = model.HomeCity;
                homeLocation.State = model.HomeState;
                homeLocation.Street = model.HomeStreet;

                locService.AddLocation(homeLocation);
                user.HomeTownLocationId = homeLocation.Id;




                school.Name = model.SchoolName;
                school.GraduationYear = model.GraduationYear;

                schoolService.AddSchool(school);
                user.SchoolId = school.Id;



                work.CompanyName = model.CompanyName;
                work.Position = model.Position;
                work.Description = model.Description;
                work.StartDate = model.StartDate;
                work.EndDate = model.EndDate;

                workPlaceService.AddWorkPlace(work);
                user.WorkPlaceId = work.Id;





                user.Name = model.Name;
                user.Firstname = model.Firstname;
               
                user.DateOfBirthday = model.DateOfBirthday;
                user.FamilyStatusId = model.SelectedStatus;
                user.Gender = model.Gender;

                userService.EditUser(user);
              
                return RedirectToAction("Index","Users");

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
                Images headerImage=new Images();
                var user = userService.SearchUser(GetId());

           
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(imageFile.InputStream))
                {
                    imageData = binaryReader.ReadBytes(imageFile.ContentLength);
                }
                headerImage.Name = imageFile.FileName;
                headerImage.Data = imageData;
                headerImage.ContentType = imageFile.ContentType;

                imgService.InsertImage(headerImage);

                user.ImagesId = headerImage.Id;
                userService.EditUser(user);

                return RedirectToAction("Index", "Users");
            }

            return HttpNotFound();

        }






        // GET: Users/Delete/5
        public ActionResult Delete(int id)
        {
            var user = userService.SearchUser(id);
            return View("Delete",user);
        }

        // POST: Users/Delete/5
        [HttpPost]
        public ActionResult Delete(UserDetails u)
        {
            try
            {
                var user = userService.SearchUser(u.Id);
                var USER = db.AspNetUsers.Find(user.UserId);


                //if (u.ImagesId != null)
                //{
                //    var photo = imgServ.SearchImg(u.ImagesId);
                //    imgServ.DeleteImage(photo);
                //}

                userService.DeleteUser(user);


                if (USER.Id!=null)
                db.AspNetUsers.Remove(USER);
                
                db.SaveChanges();

                return RedirectToAction("LogOff", "Account");
            }
            catch (Exception e)
            {
                return View();
            }
        }







        public UserDetails GetUser()
        {
            var user = userService.SearchByUserId(User.Identity.GetUserId());
            return user;
        }

        public int GetId()
        {
            var user = userService.SearchByUserId(User.Identity.GetUserId());
            return user.Id;
        }



        public byte[] DefaultPhoto()
        {
            var photo = imgService.SearchImg(1058);

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







        //public ActionResult AddPhoto(HttpPostedFileBase img)
        //{
        //   // int id=0;
        //    if (img != null)
        //    {

        //        byte[] imageData = null;
        //        using (var binaryReader = new BinaryReader(img.InputStream))
        //        { 
        //            imageData = binaryReader.ReadBytes(img.ContentLength);
        //        }
        //        var headerImage = new Images()
        //        {
        //            Name = img.FileName,
        //            Data= imageData,
        //            ContentType = img.ContentType
        //        };
        //        imgService.InsertImage(headerImage);

        //    }

        //    return View();
        //}


































































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

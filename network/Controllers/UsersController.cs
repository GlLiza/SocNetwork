using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.DAL.IRepository;
using network.DAL.Repository;
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




        private IImagesRepository imgRepository;

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
            string id = User.Identity.GetUserId();
            var users = userService.GetUser(id).ToList();

            List<ShowUserViewModel> model=new List<ShowUserViewModel>();

            foreach (var b in users)
            {
                ShowUserViewModel user=new ShowUserViewModel();

                user.Id = b.Id;
                user.Firstname = b.Firstname;
                //user.Image = b.Images.Data;
                //user.User = b.AspNetUsers;

                var friend = friendServ.SearchByFriend(b.UserId);

                if (friend != null)
                    //user.User = b.AspNetUsers;
                model.Add(user);

            }
            return View(model);
        }


        // GET: Users/Details/5
        public ActionResult Details(UsersDetailsViewModel model)
        {
            var user = userService.SearchByUserId(model.Id);

            IEnumerable<Requests> listfriends = friendServ.CurrentRequestses(model.Id);

            model.Requests = listfriends;
            model.UserDetails = user;
            int a = model.Requests.Count();
           
            return PartialView(a);
        }



        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            UserDetails user = userService.SearchUser(id);
            
            var model = new CreateUserViewModel
            {
                Id=user.Id,
                UserId = user.UserId,

            };
            return PartialView("Edit",model);
        }




        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase img, CreateUserViewModel model)
      {
            try
            {
                Location currentLoc=new Location();
                Location homeLocation=new Location();
                WorkPlace work=new WorkPlace();
                School school=new School();
                Images headerImage = new Images();

                UserDetails user = userService.SearchUser(model.Id);



                if (img != null)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(img.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(img.ContentLength);
                        }
                        headerImage.Name = img.FileName;
                        headerImage.Data = imageData;
                        headerImage.ContentType = img.ContentType;

                        imgService.InsertImage(headerImage);

                        user.ImagesId = headerImage.Id;
                        userService.EditUser(user);
                    }


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
                user.GenderId = model.SelectedGender;

                userService.EditUser(user);


                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
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




        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult AddPhoto(HttpPostedFileBase img)
        {
           // int id=0;
            if (img != null)
            {
                
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(img.InputStream))
                { 
                    imageData = binaryReader.ReadBytes(img.ContentLength);
                }
                var headerImage = new Images()
                {
                    Name = img.FileName,
                    Data= imageData,
                    ContentType = img.ContentType
                };
                imgService.InsertImage(headerImage);

            }

            return View();
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



    }
}

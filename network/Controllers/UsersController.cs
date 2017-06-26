using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public ImageService imgServ;
        private IImagesRepository imgRepository;

        public UsersController()
        {
            userService = new UserService();
            imgRepository=new ImagesRepository(db);
        }

        [HttpGet]
        public ActionResult Index()
        {
            //var users = userService.GetUser().ToList();
            return View(db.UserDetails.ToList());
        }


        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            var user = userService.SearchUser(id);
            return View(user);
        }

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

        // GET: Users/Edit/5
        public ActionResult Edit(int id)
        {
            UserDetails user = userService.SearchUser(id);

            var model = new CreateUserViewModel
            {
                Id=user.Id,
                UserId = user.UserId,
                SelectedStatus = user.FamilyStatusId,
                FamStat = user.FamilyStatus,
                FamilyStatus = db.FamilyStatus.ToList(),
                Name = user.Name,
                Firstname = user.Firstname,
                DateOfBirthday = user.DateOfBirthday,
                Country = user.Country,
                City=user.City,
                Image =user.Images

            };
            return View("Edit",model);
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase img,CreateUserViewModel model)
        {
            try
            {
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
                        
                        imgRepository.AddImage(headerImage);

                        imgRepository.Save();

                        user.ImagesId = headerImage.Id;
                        userService.EditUser(user);
                    }
              
                user.Name = model.Name;
                user.Firstname = model.Firstname;
                user.City = model.City;
                user.Country = model.Country;
                user.DateOfBirthday = model.DateOfBirthday;
                user.FamilyStatusId = model.SelectedStatus;

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



        // GET: Users/Create
        public ActionResult CreatePhotoalbum(int id)
        {
            UserDetails user = userService.SearchUser(id);


            return View("Create");
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult CreatePhotoalbum(HttpPostedFileBase img, UserDetails user)
        {
            try
            { 


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

               return RedirectToAction("Index");
            }
            catch (Exception c)
            {
                return View();
            }
        }

        //public ActionResult GetImage(int id)
        //{

        //    byte[] imageData = userService.ReturnImage(id);
        //    var img = new FileStreamResult(new System.IO.MemoryStream(imageData), "image/jpeg");
        //    return img;
        //}


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
                imgServ.InsertImage(headerImage);

            }

            return View();
        }

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

    }
}

using network.BLL;
using network.BLL.EF;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace network.Controllers
{
    public class ImgAlbumController : Controller
    {

        public PhAlbumService AlbumServ;
        public UserService UserServ;
        public ImageService ImgServ;

        
        public ImgAlbumController()
        {
            AlbumServ = new PhAlbumService();
            UserServ = new UserService();
            ImgServ = new ImageService();
            
        }

        // GET: Album
        public ActionResult Index()
        {
            var listAlbums = AlbumServ.GetListAlbums(GetId());
            return View(listAlbums);
        }



        // GET: Album/Details/5
        public ActionResult Details(int id)
        {
            
            return View();
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: Album/Create
        [HttpPost]
        public ActionResult Create(Photoalbum alb)
        {
            try
            {
                alb.UserId = GetId();
                AlbumServ.AddNewAlbum(alb);

                return RedirectToAction("Index","Users");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: Album/Edit/5
        public ActionResult Edit(int id)
        {
            var album = AlbumServ.SearchAlbum(id);

            return View("Edit",album);
        }

        // POST: Album/Edit/5
        [HttpPost]
        public ActionResult Edit( Photoalbum album)
        {
            try
            {
                AlbumServ.EditAlbum(album);
                return RedirectToAction("Index","Users");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Album/Delete/5
        public ActionResult Delete(int id)
        {
            var alb = AlbumServ.SearchAlbum(id);
            return View("Delete", alb);
        }

        // POST: Album/Delete/5
        [HttpPost]
        public ActionResult Delete(Photoalbum alb)
        {
            try
            {
               
                AlbumServ.DeleteAlbum(alb);

                return RedirectToAction("Index","Users");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult BrowseAlbums(int id)
        {
            var user = UserServ.SearchUser(id);
            var photalbum = AlbumServ.GetListAlbums(user.Id);

            return View(photalbum);
        }



        public ActionResult AddPhoto(int id)
        {
            //var al = albumServ.SearchAlbum(id);
            return View("AddPhoto");
        }

        [HttpPost]
        public ActionResult AddPhoto(int id, HttpPostedFileBase img)
        {
            try
            {
                Photoalbum album = AlbumServ.SearchAlbum(id);
                Images headerImage = new Images();
                AlbAndPhot entry=new AlbAndPhot();
                entry.PhotoalbumId = album.Id;



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

                    ImgServ.InsertImage(headerImage);
                    entry.ImageId = headerImage.Id;
                    
                    AlbumServ.AddNewEntry(entry);
                }

                return RedirectToAction("Index","Users");
            }

            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult DeletePhoto(int id)
        {
            Images img = ImgServ.SearchImg(id);
            return View("Deletephoto", img);
        }

        [HttpPost]
        public ActionResult DeletePhoto(Images img)
        {
            AlbumServ.DeletePhoto(img.Id);
            return RedirectToAction("Index", "Users");
        }

        public PartialViewResult OpenAlbum(int id)
        {
            var photos = AlbumServ.OpenAlbum(id);
            return PartialView("OpenAlbum",photos);
        }


        public int GetId()
        {
            var user = UserServ.SearchByUserId(User.Identity.GetUserId());
            return user.Id;
        }



        ////set default empty img
        //public byte[] DefaultPhoto()
        //{
        //    var photo = ImgServ.SearchImg(1058);

        //    return photo.Data;
        //}

    }
}

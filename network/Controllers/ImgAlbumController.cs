using network.BLL;
using network.BLL.EF;
using System;
using System.IO;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.Views.ViewModels;

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
            ImgViewModel model=new ImgViewModel();
            var listAlbums = AlbumServ.GetListAlbums(GetId());
            model.Albums = listAlbums;

            var listPhotos = AlbumServ.GetAllImg(GetId());
            model.Images = listPhotos;


            return View(model);
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

                return RedirectToAction("Index","ImgAlbum");
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

            return PartialView("_Edit",album);
        }
        
        // POST: Album/Edit/5
        [HttpPost]
        public ActionResult Edit( Photoalbum album)
        {
            try
            {
                if (album != null)
                {

                    AlbumServ.EditAlbum(album);
                    return RedirectToAction("OpenAlbum", "ImgAlbum", new {id = album.Id});
                }

            }

            catch (Exception ex)
            {
                return View();
            }
            return HttpNotFound();
        }



        public ActionResult DeletePhoto(int id)
        {
            Images img = ImgServ.SearchImg(id);
            return PartialView("_Deletephoto", img);
        }

        [HttpPost]
        public ActionResult DeletePhoto(Images img)
        {
            AlbumServ.DeletePhoto(img.Id);
            return RedirectToAction("Index", "Users");
        }

        // GET: Album/Delete/5
        public ActionResult Delete(int id)
        {
            var alb = AlbumServ.SearchAlbum(id);
            return PartialView("Delete", alb);
        }
        
        // POST: Album/Delete/5
        [HttpPost]
        public ActionResult Delete(Photoalbum alb)
        {
            try
            {
               
                AlbumServ.DeleteAlbum(alb);

                return RedirectToAction("Index","ImgAlbum");
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
            AddPhotoViewModel model=new AddPhotoViewModel();
            model.Id = id;
            return PartialView("_AddPhoto",model);
        }
    
        [HttpPost]
        public ActionResult AddPhoto(AddPhotoViewModel model)
        {
            try
            {
                Photoalbum album = AlbumServ.SearchAlbum(model.Id);
                Images headerImage = new Images();
                AlbAndPhot entry=new AlbAndPhot();
                entry.PhotoalbumId = album.Id;



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
                    headerImage.Date=DateTime.Now;

                    ImgServ.InsertImage(headerImage);
                    entry.ImageId = headerImage.Id;
                    
                    AlbumServ.AddNewEntry(entry);
                }

                return RedirectToAction("OpenAlbum","ImgAlbum",new {id=model.Id});
            }

            catch (Exception ex)
            {
                return View();
            }
        }
        

        //позволяет открыть альбом
        public ActionResult OpenAlbum(int id)
        {
            OpenAlbumViewModel model = new OpenAlbumViewModel();
            model.Id = id;
            model.NameAlb = AlbumServ.SearchAlbum(id).Name;
            model.Photos = AlbumServ.OpenAlbum(id);
            return View(model);

        }
        

        public int GetId()
        {
            var user = UserServ.SearchByUserId(User.Identity.GetUserId());
            return user.Id;
        }


    }
}

using network.BLL;
using network.BLL.EF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using network.Views.ViewModels;

namespace network.Controllers
{
    public class ImgAlbumController : Controller
    { 

        public PhAlbumService albumServ;
        public UserService UserServ;
        public ImageService ImgServ;

        
        public ImgAlbumController()
        {
            albumServ = new PhAlbumService();
            UserServ = new UserService();
            ImgServ = new ImageService();
            
        }

        // GET: Album
        public ActionResult Index()
        {
            ALbumsViewModel model=new ALbumsViewModel();
            


            List<AlbumViewModel> albumViewModel=new List<AlbumViewModel>();
         
            var listAlbums = albumServ.GetListAlbums(GetId()).ToList();

            foreach (var album in listAlbums)
            {
                AlbumViewModel modelAlb = new AlbumViewModel();


                modelAlb.AlbumId = album.Id;
                modelAlb.Name = album.Name;
                modelAlb.TitleImage = albumServ.GetLastImgAlbum(album.Id);


                if (modelAlb.TitleImage == null)
                    modelAlb.TitleImage = ImgServ.SearchImg(1058);


                albumViewModel.Add(modelAlb);
            }

            AlbumImgViewModel AlbImgModel=new AlbumImgViewModel();
            var listImgAlb = albumServ.GetAllImg(GetId());
            AlbImgModel.AllImages = listImgAlb;


            model.Albums = albumViewModel;
            model.Images = AlbImgModel;

            return View(model);







            //AlbumViewModel albumViewModel=new AlbumViewModel();
            //AlbumImgViewModel AlbImgModel=new AlbumImgViewModel();
            //ImgViewModel model=new ImgViewModel();



            //model.Albums = listAlbums;
            //var imgFor = AlbumServ.AlbumImg(model.Albums.ToList());
            //model.ImgForAlb = imgFor;


            //var listPhotos = AlbumServ.GetAllImg(GetId());
            //model.Images = listPhotos;
            
        }
        

        // GET: Album/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        //GET: Album/Create
        public ActionResult AddAlbum()
        {
            Photoalbum album=new Photoalbum();
            return PartialView("_AddAlbum",album);
        }

        // POST: Album/Create
        [HttpPost]
        public ActionResult AddAlbum(Photoalbum album)
        {
            try
            {
                album.UserId = GetId();
                //album.UserDetails = UserServ.SearchUser(album.UserId);
                albumServ.AddNewAlbum(album);

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
            var album = albumServ.SearchAlbum(id);

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

                    albumServ.EditAlbum(album);
                    return RedirectToAction("OpenAlbum", "ImgAlbum", new {id = album.Id});
                }

            }

            catch (Exception ex)
            {
                return View();
            }
            return HttpNotFound();
        }



        //for photo
        [HttpGet]
        public ActionResult DeletePhoto()
        {
            return PartialView("_Deletephoto");
        }

        [HttpPost]
        public String DeletePhoto(int imageId)
        {
            if (imageId != null)
                albumServ.DeletePhoto((int)imageId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            var res = new HttpStatusCodeResult(HttpStatusCode.OK);
            return js.Serialize(res);
            //return RedirectToAction("Index","ImgAlbum");
        }




        //for album
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var alb = albumServ.SearchAlbum(id);
            return PartialView("Delete", alb);
        }
        
        
        [HttpPost]
        public ActionResult Delete(Photoalbum alb)
        {
            try
            {
               
                albumServ.DeleteAlbum(alb);

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
            var photalbum = albumServ.GetListAlbums(user.Id);

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
                Photoalbum album = albumServ.SearchAlbum(model.Id);
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
                    headerImage.Visibility = true;

                    ImgServ.InsertImage(headerImage);
                    entry.ImageId = headerImage.Id;
                    
                    albumServ.AddNewEntry(entry);
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
            model.NameAlb = albumServ.SearchAlbum(id).Name;
            model.Photos = albumServ.OpenAlbum(id);
            //var im = AlbumServ.GetLastImg(id);

           return View(model);

        }
        

        public int GetId()
        {
            var user = UserServ.SearchByUserId(User.Identity.GetUserId());
            return user.Id;
        }
        
    }
}

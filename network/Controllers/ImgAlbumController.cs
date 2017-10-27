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

        private readonly PhAlbumService _albumServ;
        private readonly UserService _userServ;
        private readonly ImageService _imgServ;

        
        public ImgAlbumController(PhAlbumService albumServ,UserService userServ, ImageService imgServ)
        {
            _albumServ = albumServ;
            _userServ = userServ;
            _imgServ = imgServ;
        }

        // GET: Album
        public ActionResult Index()
        {
            ALbumsViewModel model=new ALbumsViewModel();          


            List<AlbumViewModel> albumViewModel=new List<AlbumViewModel>();
         
            var listAlbums = _albumServ.GetListAlbums(GetId()).ToList();

            foreach (var album in listAlbums)
            {
                AlbumViewModel modelAlb = new AlbumViewModel();


                modelAlb.AlbumId = album.Id;
                modelAlb.Name = album.Name;
                modelAlb.TitleImage = _albumServ.GetLastImgAlbum(album.Id);


                //if (modelAlb.TitleImage == null)
                //    modelAlb.TitleImage = _imgServ.SearchImg(1058);


                albumViewModel.Add(modelAlb);
            }

            AlbumImgViewModel AlbImgModel=new AlbumImgViewModel();
            var listImgAlb = _albumServ.GetAllImg(GetId());
            AlbImgModel.AllImages = listImgAlb;


            model.Albums = albumViewModel;
            model.Images = AlbImgModel;

            return View(model);
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
                _albumServ.AddNewAlbum(album);

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
            var album = _albumServ.SearchAlbum(id);

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

                    _albumServ.EditAlbum(album);
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
                _albumServ.DeletePhoto((int)imageId);

            JavaScriptSerializer js = new JavaScriptSerializer();
            var res = new HttpStatusCodeResult(HttpStatusCode.OK);
            return js.Serialize(res);
        }




        //for album
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var alb = _albumServ.SearchAlbum(id);
            return PartialView("Delete", alb);
        }
        
        
        [HttpPost]
        public ActionResult Delete(Photoalbum alb)
        {
            try
            {
               if (_albumServ.DeleteAlbum(alb.Id))
                return RedirectToAction("Index","ImgAlbum");
                return RedirectToAction("Index","ImgAlbum");
            }
            
            catch (Exception e)
            {
                return View();
            }
        }



        public ActionResult BrowseAlbums(int id)
        {
            var user = _userServ.SearchUser(id);
            var photalbum = _albumServ.GetListAlbums(user.Id);

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
                Photoalbum album = _albumServ.SearchAlbum(model.Id);
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

                    _imgServ.InsertImage(headerImage);
                    entry.ImageId = headerImage.Id;
                    
                    _albumServ.AddNewEntry(entry);
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
            model.NameAlb = _albumServ.SearchAlbum(id).Name;
            model.Photos = _albumServ.OpenAlbum(id);

           return View(model);

        }
        

        public int GetId()
        {
            var user = _userServ.SearchByUserId(User.Identity.GetUserId());
            return user.Id;
        }
        
    }
}

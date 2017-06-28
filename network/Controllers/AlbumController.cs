using network.BLL;
using network.BLL.EF;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace network.Controllers
{
    public class AlbumController : Controller
    {

        public NetworkContext db = new NetworkContext();
        public PhotoalbumService albumServ;
        public UserService userServ;
        public ImageService imgServ;

        
        public AlbumController()
        {
            albumServ = new PhotoalbumService();
            userServ = new UserService();
            imgServ = new ImageService();
        }

        // GET: Album
        public ActionResult Index()
        {
            return View();
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
        public ActionResult Create(int id, Photoalbum alb)
        {
            try
            {
                alb.UserId = id;
                //alb. = rnd.Next();
                albumServ.AddNewAlbum(alb);

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
            var album = albumServ.SearchAlbum(id);

            return View("Edit",album);
        }

        // POST: Album/Edit/5
        [HttpPost]
        public ActionResult Edit( Photoalbum album)
        {
            try
            {
                albumServ.EditAlbum(album);
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
            var album = albumServ.SearchAlbum(id);
            return View("Delete",album);
        }

        // POST: Album/Delete/5
        [HttpPost]
        public ActionResult Delete(Photoalbum alb)
        {
            try
            {
                albumServ.DeleteAlbum(alb);

                return RedirectToAction("Index","Users");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult BrowseAlbums(int id)
        {
            var user = userServ.SearchUser(id);
            var photalbum = albumServ.GetListAlbums(user.Id);

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
                Photoalbum album = albumServ.SearchAlbum(id);
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

                    imgServ.InsertImage(headerImage);
                    entry.ImageId = headerImage.Id;
                    
                    albumServ.AddNewEntry(entry);
                }

                return RedirectToAction("Index","Users");
            }

            catch (Exception ex)
            {
                return View();
            }
        }

        public PartialViewResult OpenAlbum(int id)
        {
            var photos = albumServ.OpenAlbum(id);
            return PartialView("OpenAlbum",photos);
        }
    }
}

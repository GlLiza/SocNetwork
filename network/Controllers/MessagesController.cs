using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.Views.ViewModels;

namespace network.Controllers
{
    public class MessagesController : Controller
    {

        public MessagesService msgService;

        public MessagesController()
        {
            msgService=new MessagesService();
        }

        // GET: Messages
        public ActionResult Index()
        {
            return View();
        }


        //GET: select a receiver
        public ActionResult SelectReceiver()
        {
            SelectReceiver receiver=new SelectReceiver();
            IQueryable<Friendship> friendships = msgService.GetFriendForSelect(User.Identity.GetUserId());
            if (friendships!=null)
            receiver.FriendsList=msgService.GetUserDetails(friendships);
            
            return PartialView("_SelectReceiver", receiver);
        }


        //POST: create a chat
        [HttpPost]
        public ActionResult SelectReceiver(Participants participants)
        {

            return View();
        }




        // GET: Messages/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Messages/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Messages/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

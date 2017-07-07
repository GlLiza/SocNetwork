using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;

namespace network.Controllers
{
    public class FriendshipController : Controller
    {
        public NetworkContext db = new NetworkContext();
        public FriendshipService friendServ;
        public UserService userService;

        public FriendshipController()
        {
            friendServ=new FriendshipService();
            userService = new UserService();
        }

        // GET: Friendship
        public ActionResult Index(string id)
        {
            var friends = friendServ.GetFriendList(id);


            return View("View", friends);
        }

        // GET: Friendship/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
           Friendship friend = friendServ.SearchByFriend(id);
            if (friend.Id != 0)
            {
                return View(friend);
            }
            return RedirectToAction("Index", "Users");

        }

        // POST: Friendship/Delete/5
        
        [HttpPost]
        public ActionResult Delete(Friendship friendship)
        {
            try
            {
                Friendship friend = friendServ.SearchFriendship(friendship.Id);
                friendServ.DeleteFriendship(friend);

                return RedirectToAction("Index","Users");
            }
            catch (Exception e)
            {
                return View();
            }
        }



     

        [HttpGet]
        public ActionResult AddFriend()
        {
            return View("AddFriend");
        }

        [HttpPost]
        public ActionResult AddFriend(AspNetUsers user2)
        {
            Friendship friend1 = new Friendship();
            Friendship friend2 = new Friendship();

            friend1.User2_id = user2.Id;
            friend1.User1_id = User.Identity.GetUserId();
            friend1.Status = true;



            friend2.User1_id = user2.Id;
            friend2.User2_id = User.Identity.GetUserId();
            friend2.Status = true;



            friendServ.AddFriendship(friend1);
            friendServ.AddFriendship(friend2);
            return RedirectToAction("Index", "Users");
        }
    }
}

// GET: Friendship/Details/5
//public ActionResult Details(int id)
//{
//    return View();
//}

// GET: Friendship/Create
//public ActionResult Create()
//{
//    return View();
//}

// POST: Friendship/Create
//[HttpPost]
//public ActionResult Create(FormCollection collection)
//{
//    try
//    {
//        // TODO: Add insert logic here

//        return RedirectToAction("Index");
//    }
//    catch
//    {
//        return View();
//    }
//}

// GET: Friendship/Edit/5
//public ActionResult Edit(int id)
//{
//    return View();
//}

// POST: Friendship/Edit/5
//[HttpPost]
//public ActionResult Edit(int id, FormCollection collection)
//{
//    try
//    {
//        // TODO: Add update logic here

//        return RedirectToAction("Index");
//    }
//    catch
//    {
//        return View();
//    }
//}



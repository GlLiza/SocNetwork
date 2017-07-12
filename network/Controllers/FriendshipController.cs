using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.Views.ViewModels;

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




        // send request to friend

        public ActionResult SendRequest( string id)
        {
            Requests request=new Requests();
            request.Requesting_user_id =id;
            request.Requested_user_id = User.Identity.GetUserId();
            request.Status_id = 1;
            request.Date_requsted=DateTime.Now;
            friendServ.AddRequest(request);
            return RedirectToAction("Index", "Users");
        }


        public ActionResult AcceptRequest (Requests requests)       
        {
            Requests req = friendServ.SearchRequest(requests.Id);
            req.Status_id = 3;
            friendServ.UpdateRequest(req);

            Friendship friendship1=new Friendship();
            friendship1.User_id = req.Requesting_user_id;
            friendship1.Friend_id = req.Requested_user_id;

            Friendship friendship2 = new Friendship();
            friendship2.User_id = req.Requested_user_id; 
            friendship2.Friend_id = req.Requesting_user_id;

            friendServ.AddFriendship(friendship1);
            friendServ.AddFriendship(friendship2);
            

            return RedirectToAction("Index","Users");
        }

        public ActionResult RejectrRequest(Requests requests)
        {
            Requests req = friendServ.SearchRequest(requests.Id);
            req.Status_id = 2;
            friendServ.UpdateRequest(req);

            return RedirectToAction("Index", "Users");
        }

        public ActionResult IgnoreRequest(Requests requests)
        {
            Requests req = friendServ.SearchRequest(requests.Id);
            req.Status_id = 4;
            friendServ.UpdateRequest(req);

            return RedirectToAction("Index", "Users");
        }
    }
}






//[HttpGet]
//public ActionResult AddFriend()
//{
//    return View("AddFriend");
//}

//[HttpPost]
//public ActionResult AddFriend(AspNetUsers user2)
//{
//    Friendship friend1 = new Friendship();
//    Friendship friend2 = new Friendship();

//    //friend1.User2_id = user2.Id;
//    //friend1.User1_id = User.Identity.GetUserId();
//    //friend1.Status = true;



//    //friend2.User1_id = user2.Id;
//    //friend2.User2_id = User.Identity.GetUserId();
//    //friend2.Status = true;



//    friendServ.AddFriendship(friend1);
//    friendServ.AddFriendship(friend2);
//    return RedirectToAction("Index", "Users");
//}






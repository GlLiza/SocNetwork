using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using network.BLL;
using network.BLL.EF;
using network.Views.ViewModels;
using System.Linq;

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
        public ActionResult Index()
        {
            List<ShowUserViewModel> model = new List<ShowUserViewModel>();

            var frList = friendServ.GetFriendList(User.Identity.GetUserId());

            if (frList != null)
            {
                foreach (var b in frList)
                {
                    ShowUserViewModel userr = new ShowUserViewModel();

                    var user = userService.SearchByUserId(b.Friend_id);

                    userr.Id = user.Id;
                    userr.Firstname = user.Firstname;
                    userr.Name = user.Name;
                    userr.Image = user.Images.Data;
                    userr.User = user.AspNetUsers;
                    user.AspNetUsers = b.AspNetUsers;

                    model.Add(userr);
                }
                return View(model);
            }
            return RedirectToAction("Index", "Users");
        }


        public ActionResult FriendsOfFriends(int id)
        {
            List<ShowUserViewModel> model = new List<ShowUserViewModel>();

            var friend = userService.SearchUser(id);

            var frOffr = friendServ.GetFriendList(friend.UserId);

            if (frOffr != null)
            {
                foreach (var b in frOffr)
                {
                    ShowUserViewModel userr = new ShowUserViewModel();

                    var user = userService.SearchByUserId(b.Friend_id);

                    userr.Id = user.Id;
                    userr.Firstname = user.Firstname;
                    userr.Name = user.Name;
                    userr.Image = user.Images.Data;
                    userr.User = user.AspNetUsers;
                    user.AspNetUsers = b.AspNetUsers;

                    model.Add(userr);
                }
                return View(model);
            }
            return RedirectToAction("Index", "Users");

        }


        //GET:count requests
        public ActionResult CountRequests()
        {
            UsersDetailsViewModel model=new UsersDetailsViewModel();
            model.Id = User.Identity.GetUserId();
         
            var user = userService.SearchByUserId(model.Id);

            IQueryable<Requests> listfriends = friendServ.CurrentRequestses(model.Id);
            model.Requests = listfriends;

            model.UserDetails = user;
            //int a = model.Requests.Count();

            if(model.Requests.Count()> 0)
                return PartialView(model.Requests.Count());
            return HttpNotFound();

        }


        // show new requests 
        public ActionResult NewRequest()
        {
            List<NewRequestsViewModel> model = new List<NewRequestsViewModel>();

            var newFr = friendServ.RequestList(User.Identity.GetUserId());

            if (newFr != null)
            {
                foreach (var b in newFr)
                {
                    NewRequestsViewModel user = new NewRequestsViewModel();
                    var requestedUser = userService.SearchByUserId(b.Requested_user_id);
                    var request = friendServ.SearchUsers(b.Requesting_user_id, b.Requested_user_id);

                    user.Id = requestedUser.UserId;
                    user.Name = requestedUser.Name;
                    user.Firstname = requestedUser.Firstname;
                    user.Image = requestedUser.Images.Data;
                    user.Requests = request;

                    model.Add(user);
                }

                return View(model);
            }

            return RedirectToAction("Index", "Friendship");

        }

        public ActionResult SendRequest(string id)     //отправить запрос
        {
            if (CheckRequest(id) == false)
            {
                Requests request = new Requests();
                request.Requesting_user_id = id;
                request.Requested_user_id = User.Identity.GetUserId();
                request.Status_id = 1;
                request.Date_requsted = DateTime.Now;
                friendServ.AddRequest(request);

                TempData["message"] = "Request has been sent to user";

                return RedirectToAction("BrowseUsers", "Users");
            }

            TempData["message"] = "Request has't been sent to user";

           return RedirectToAction("BrowseUsers", "Users");
        }


        public bool CheckRequest(string id)
        {
            string Id = User.Identity.GetUserId();
            return friendServ.Check(Id, id);
        }
        
        
       
        public ActionResult AcceptRequest(int requestsId)
        {
            Requests req = friendServ.SearchRequest(requestsId);
            req.Status_id = 3;
            //!!!!!!!!!!
            //friendServ.Save();
           
            Friendship friendship1 = new Friendship();
            friendship1.User_id = req.Requesting_user_id;
            friendship1.Friend_id = req.Requested_user_id;

            Friendship friendship2 = new Friendship();
            friendship2.User_id = req.Requested_user_id;
            friendship2.Friend_id = req.Requesting_user_id;

            friendServ.AddFriendship(friendship1);
            friendServ.AddFriendship(friendship2);


            return RedirectToAction("Index", "Friendship");
        }

        
        public ActionResult RejectrRequest(int id)
        {
            Requests req = friendServ.SearchRequest(id);
            req.Status_id = 2;
            //!!!!!!!!!!!
            //friendServ.Save();

            return RedirectToAction("Index", "Friendship");
        }


        public ActionResult IgnoreRequest(int id)
        {
            Requests req = friendServ.SearchRequest(id);
            req.Status_id = 4;
            //!!!!!!!!
            //friendServ.Save();
            

            return RedirectToAction("Index", "Friendship");
        }






        //delete friendship

        //GET
        public ActionResult Delete(int id)
        {
            var us= userService.SearchUser(id);
            return PartialView("_DeleteFriends",us.UserId);
        }


        [HttpPost]
        public ActionResult Delete(string Id)
        {

            Friendship friendship = friendServ.SearchByUsers(User.Identity.GetUserId(), Id);

            if (friendship == null) return HttpNotFound();
            friendServ.DeleteFriendship(friendship.Id);
            return RedirectToAction("Index", "Friendship");
        }
        

    }
}


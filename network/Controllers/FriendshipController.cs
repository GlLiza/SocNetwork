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
        private readonly FriendshipService _friendServ;
        private readonly UserService _userService;

    public FriendshipController(FriendshipService friendServ, UserService userService)
        {
        _friendServ = friendServ;
        _userService = userService;
            }

        // GET: Friendship
    public ActionResult Index()
    {
        List<ShowUserViewModel> model = new List<ShowUserViewModel>();

        var frList = _friendServ.GetFriendList(User.Identity.GetUserId());

        if (frList != null)
        {
            foreach (var b in frList)
            {
                ShowUserViewModel userr = new ShowUserViewModel();

                var user = _userService.SearchByUserId(b.Friend_id);

                userr.Id = user.Id;
                userr.Firstname = user.Firstname;
                userr.Name = user.Name;
                userr.Image = user.Images.Data;
                userr.User = user.AspNetUsers;
                //userr.AspNetUsers = b.AspNetUsers;

                    model.Add(userr);
            }
            return View(model);
        }
        return RedirectToAction("Index", "Users");
    }


    public ActionResult FriendsOfFriends(int id)
    {
        List<ShowUserViewModel> model = new List<ShowUserViewModel>();

        var friend = _userService.SearchUser(id);

        var frOffr = _friendServ.GetFriendList(friend.UserId);

        if (frOffr != null)
        {
            foreach (var b in frOffr)
            {
                ShowUserViewModel userr = new ShowUserViewModel();

                var user = _userService.SearchByUserId(b.Friend_id);

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
         
        var user = _userService.SearchByUserId(model.Id);

        IQueryable<Requests> listfriends = _friendServ.CurrentRequestses(model.Id);
        model.Requests = listfriends;

        model.UserDetails = user;

        if(model.Requests.Count()> 0)
            return PartialView(model.Requests.Count());
        return HttpNotFound();

    }


    // show new requests 
    public ActionResult NewRequest()
    {
        List<NewRequestsViewModel> model = new List<NewRequestsViewModel>();

        var newFr = _friendServ.RequestList(User.Identity.GetUserId());

        if (newFr != null)
        {
            foreach (var b in newFr)
            {
                NewRequestsViewModel user = new NewRequestsViewModel();
                var requestedUser = _userService.SearchByUserId(b.Requested_user_id);
                var request = _friendServ.SearchUsers(b.Requesting_user_id, b.Requested_user_id);

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
        if (CheckRequest(id,User.Identity.GetUserId()) == false)
        {
                if (CheckRequest(User.Identity.GetUserId(), id) == false)
                {
                    Requests request = new Requests();
                    request.Requesting_user_id = id;
                    request.Requested_user_id = User.Identity.GetUserId();
                    request.Status_id = 1;
                    request.Date_requsted = DateTime.Now;
                    _friendServ.AddRequest(request);
                    TempData["message"] = "Request has been sent to user";

                    return RedirectToAction("BrowseUsers", "Users");
                } 
        }

        TempData["message"] = "Request has't been sent to user";

        return RedirectToAction("BrowseUsers", "Users");
    }


    public bool CheckRequest(string idIng,string idEd)
    {       
        return _friendServ.Check(idEd, idIng);
    }  


    public ActionResult AcceptRequest(int requestsId)
    {
        _friendServ.StatusToAccepted(requestsId);
        Requests req = _friendServ.SearchRequest(requestsId);
                     

        Friendship friendship1 = new Friendship();
        friendship1.User_id = req.Requested_user_id ;
        friendship1.Friend_id = req.Requesting_user_id;

        Friendship friendship2 = new Friendship();
        friendship2.User_id = req.Requesting_user_id;
        friendship2.Friend_id = req.Requested_user_id;

        _friendServ.AddFriendship(friendship1);
        _friendServ.AddFriendship(friendship2);


        return RedirectToAction("Index", "Friendship");
    }

        
    public ActionResult RejectrRequest(int id)
    {
        _friendServ.StatusToDeclined(id);    
        return RedirectToAction("Index", "Friendship");
    }


    public ActionResult IgnoreRequest(int id)
    {
        _friendServ.StatusToIgnored(id);
        return RedirectToAction("Index", "Friendship");
    }






    //delete friendship

    //GET
    public ActionResult Delete(int id)
    {
        var us= _userService.SearchUser(id);
        return PartialView("_DeleteFriends",us.UserId);
    }


    [HttpPost]
    public ActionResult Delete(string Id)
    {

        Friendship friendship = _friendServ.SearchByUsers(User.Identity.GetUserId(), Id);

        if (friendship == null) return HttpNotFound();
        _friendServ.DeleteFriendship(friendship.Id);
        return RedirectToAction("Index", "Friendship");
    }
        

    }
}


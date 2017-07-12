using System.Web.Mvc;

namespace network.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]       //аттрибут указывает, что метод вызывается только если пользователь аутенцифицирован
        public ActionResult Index()
        {
            return View();
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HPStask.App_Code;
namespace HPStask.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string user,string pass)
        {
            LoginOperation op = new LoginOperation();
            if(user!=string.Empty && pass != string.Empty)
            {
                int result = op.login(user, pass);
                if (result>0)
                {
                    Session["user"] = result;
                    return RedirectToAction("index", "home");
                }
            }
            
            ViewBag.err = "wrong user or password";            
            
            return View();
        }

        public ActionResult ForgetPassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string mail)
        {
            LoginOperation op = new LoginOperation();
            op.resetPass(mail);
            ViewBag.sent = "E-mail sent to your email";
            return View();
        }

        public ActionResult logout()
        {
            Session["user"] = null;
            return RedirectToAction("login", "account");
        }
    }
}
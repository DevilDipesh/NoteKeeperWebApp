using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteKeeperWebApp.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        NoteKeeperRepo repo = new NoteKeeperRepo();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.UserAccess model)
        {
            bool log = repo.validateUser(model.EmailID, model.Password);
            if (log)
            {
                Session["User"] = model.EmailID;
                Session["UserID"] = model.ID;
                if (repo.searchUser(model.EmailID))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("saveUserDetails", "Home");

            }
            else
                ViewBag.ErrMsg = "Invalid Login Attempt";
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Models.RegisterUser model)
        {
            if(model.Password == model.ConfirmPassword)
            {
                UserAccess entity = new UserAccess();
                entity.EmailID = model.Email;
                entity.Password = model.Password;
                bool updated = repo.addUser(entity);
                if (updated)
                    return View("Login");
                else
                    return View("Error");
            }
            else
            {
                ViewBag.ErrMsg = "The password and confirmation password do not match.";
                return View(model);
            } 
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public JsonResult EmailExists(string emailID)
        {
            bool sts = repo.emailIDExists(emailID);
            return Json(sts, JsonRequestBehavior.AllowGet);
        }
        
    }
}
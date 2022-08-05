using FacturacionWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FacturacionWeb.Controllers
{
    public class AccountsController : Controller
    {
        [AllowAnonymous]
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel userModel)
        {
            //userModel.UserName = "user";
            //userModel.UserPassword = "user2002";
            if (ModelState.IsValid)
            {
                using (BDFacturaEntities contex = new BDFacturaEntities())
                {
                    bool IsValidUser = contex.Users.Any(user => 
                                user.UserName.ToLower() == userModel.UserName.ToLower() 
                                && user.UserPassword == userModel.UserPassword
                                && user.Estado == 1);
                    if (IsValidUser)
                    {
                        FormsAuthentication.SetAuthCookie(userModel.UserName, false);
                        return RedirectToAction("Index", "Principal");
                    }

                    ViewBag.Message = "UserName or password son incorrectos";
                    return View();
                }

            }
            else
            {
                return View();
            }
            
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KURSOTOMASYON.Models;

namespace KURSOTOMASYON.Controllers
{
    public class FakeLoginController : Controller
    {
        Model1 db = new Model1();
        // GET: FakeLogin
        public ActionResult Index()
        {
            YONETICI yonetici = new YONETICI();
            //otologin
            return View(yonetici);
        }
        public ActionResult Giris(YONETICI y)
        {
            YONETICI yonetici = db.YONETICIs.Where(y1=>y1.YONETICI_ADI==y.YONETICI_ADI && y1.PAROLA==y.PAROLA).SingleOrDefault();
            if (yonetici ==null)
            {
                TempData["mesaj"] = "kullanıcı adı veya parola yanlış";
                FormsAuthentication.SignOut();
                return RedirectToAction("Index");
            }

            FormsAuthentication.SetAuthCookie(yonetici.YONETICI_ADI, false);
            Session["YONETICI_REFNO"] = yonetici.YONETICI_REFNO;

            return RedirectToAction("Index", "Home");
        }
    }
}
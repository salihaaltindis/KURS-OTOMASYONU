using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KURSOTOMASYON.Models;

namespace KURSOTOMASYON.Controllers
{
    public class GirisController : Controller
    {
        // GET: Giris

        Model1 db = new Model1();
        //public ActionResult Index()
        //{

        //    YONETICI yonetici = new YONETICI();
        //    //otologin
        //    return View(yonetici);
        //    //return View();
        //}


        //public ActionResult CheckUser(string KULLANICI_ADI, string PAROLA)//INDEXTE YAZANIN ID NIN AYNISI OLMAK ZORUNDA.
        //{
        //    YONETICI kullanici = db.YONETICIs.Where(k => k.YONETICI_ADI == KULLANICI_ADI && k.PAROLA == PAROLA && k.DURUMU == true).SingleOrDefault();//eğer kullanıcı gelmiyorsa null olacak.
        //    if (kullanici != null)
        //    {

        //        FormsAuthentication.SetAuthCookie(KULLANICI_ADI, false);//giriş yapıldığı cookide tutulmasın diye false dedik.
        //        return RedirectToAction("GirisOK", "Giris");
        //    }
        //    else
        //    {
        //        return RedirectToAction("index");
        //    }
        //}

        //public ActionResult Giris(YONETICI y)
        //{
        //    YONETICI yonetici = db.YONETICIs.Where(y1 => y1.YONETICI_ADI == y.YONETICI_ADI && y1.PAROLA == y.PAROLA).SingleOrDefault();
        //    if (yonetici == null)
        //    {
        //        TempData["mesaj"] = "kullanıcı adı veya parola yanlış";
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Index");
        //    }

        //    FormsAuthentication.SetAuthCookie(yonetici.YONETICI_ADI, false);
        //    Session["YONETICI_REFNO"] = yonetici.YONETICI_REFNO;

        //    return RedirectToAction("Index", "Home");
        //}



        public ActionResult GirisOK()
        {
            return View();
        }

        public ActionResult MagicalIndex()
        {
            YONETICI yonetici = new YONETICI();
            //otologin
            return View(yonetici);
            //return View();
        }
        public ActionResult MagicalGiris(YONETICI y)
        {
            YONETICI yonetici = db.YONETICIs.Where(y1 => y1.YONETICI_ADI == y.YONETICI_ADI && y1.PAROLA == y.PAROLA).SingleOrDefault();
            if (yonetici == null)
            {
                TempData["mesaj"] = "kullanıcı adı veya parola yanlış";
                FormsAuthentication.SignOut();
                return RedirectToAction("MagicalIndex");
            }

            FormsAuthentication.SetAuthCookie(yonetici.YONETICI_ADI, false);
            Session["YONETICI_REFNO"] = yonetici.YONETICI_REFNO;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("MagicalIndex");
        }
    }
}
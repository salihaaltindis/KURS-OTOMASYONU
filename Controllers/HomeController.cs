using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KURSOTOMASYON.Models;

namespace KURSOTOMASYON.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        Model1 db = new Model1();
        public ActionResult Index()
        {
            //kayıtlı kursiyer sayısını görmek için
            int kursiyersayisi = db.KURSIYERs.Count();
            ViewData["kursiyersayisi"] = kursiyersayisi;

            //kayıtlı eğitmen sayısını görmek için
            int egitmensayisi = db.EGITMEN.Count();
            ViewData["egitmensayisi"] = egitmensayisi;

            //ön kayıtlı kursiyer sayısını görmek için
            int onkayitkisi = db.ON_KAYIT.Count();
            ViewData["onkayitkisi"] = onkayitkisi;

            //ön kayıtlı kursiyer sayısını görmek için
            int totalpara = db.KURSIYER_HAREKET.Sum(t1=>t1.ODENEN);
            ViewData["totalpara"] = totalpara;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
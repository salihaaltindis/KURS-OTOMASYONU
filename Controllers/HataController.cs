using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KURSOTOMASYON.Controllers
{
    public class HataController : Controller
    {
        // GET: Hata
        public ActionResult Index(string mesaj)
        {
            //silerken oluşan hataları basmak için olusturulan controller
            ViewData["mesaj"] = mesaj;
            return View();
        }
    }
}
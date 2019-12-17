using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KURSOTOMASYON.Models;

namespace KURSOTOMASYON.Controllers
{
    [Authorize]
    public class YetkiGruplariController : Controller
    {
        Model1 db = new Model1();
        // GET: YetkiGruplari
        public ActionResult Index()
        {
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);

            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;

            int sayfa_refno = db.SAYFAs.Where(s=>s.SAYFA_ADI == "YetkiGrubu").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr,sayfa_refno,YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index","Home");
            var liste = db.YETKI_GRUBU.ToList();
            return View(liste);
        }

        public ActionResult Yetkiler(int id)//yetki grubu id
        {
            //yetki sayfası kaydetme
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);

            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "YetkiGrubu").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");
            ViewData["id"] = id;
            ViewData["sayfalar"] = db.SAYFAs.ToList();//sayfa grubu id

            var liste = db.YETKIs.Where(y => y.YETKI_GRUBU_REFNO == id).ToList();

            return View(liste);
        }

        public ActionResult Kayit(FormCollection formdata)
        {
            //yetki sayfası kaydetme
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);

            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "YetkiGrubu").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.KAYDET);
            if (yetki == false) return RedirectToAction("Index", "Home");

            //eski yetkileri silmeliyiz.
            int YETKI_GRUBU_REFNO = Convert.ToInt32(formdata["YETKI_GRUBU_REFNO"]);
            var yetkiler = db.YETKIs.Where(y => y.YETKI_GRUBU_REFNO == YETKI_GRUBU_REFNO);
            db.YETKIs.RemoveRange(yetkiler);
            db.SaveChanges();

            int toplamsatir = db.SAYFAs.Count();

            for (int i = 0; i < toplamsatir; i++)
            {
                int sayfarefno = db.SAYFAs.ToList()[i].SAYFA_REFNO;

                bool OKUMA = false, KAYDET = false, SIL = false, YENI = false, ARAMA = false;
                if (formdata["OKUMA"+sayfarefno] != null) OKUMA = true;
                
                if (formdata["KAYDET" + sayfarefno] != null) KAYDET = true;
                
                if (formdata["SIL" + sayfarefno] != null) SIL = true;
                
                if (formdata["YENI" + sayfarefno] != null) YENI = true;
                
                if (formdata["ARAMA" + sayfarefno] != null) ARAMA = true;

                YETKI y = new YETKI()
                {
                    SAYFA_REFNO = sayfarefno,
                    YETKI_GRUBU_REFNO = YETKI_GRUBU_REFNO,
                    OKUMA = OKUMA,
                    KAYDET = KAYDET,
                    SIL = SIL,
                    ARAMA = ARAMA,
                    YENI = YENI
                };
                db.YETKIs.Add(y);
                db.SaveChanges();

            }

            return RedirectToAction("Index");
        }
    }
}
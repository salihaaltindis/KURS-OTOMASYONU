using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KURSOTOMASYON.Models;
using OfficeOpenXml;
using Rotativa;

namespace KURSOTOMASYON.Controllers
{
    public class YoneticilerController : Controller
    {
        private Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;

        // GET: Yoneticiler
        //public ActionResult Index()
        //{
        //    var yONETICIs = db.YONETICIs.Include(y => y.YETKI_GRUBU);
        //    return View(yONETICIs.ToList());
        //}
        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            ////Giris kontrolü için
            //int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            //int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            //int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "Yoneticiler").SingleOrDefault().SAYFA_REFNO;
            //bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            //if (yetki == false) return RedirectToAction("Index", "Home");


            List<YONETICI> liste = new List<YONETICI>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.YONETICIs.Count());
                liste = db.YONETICIs.OrderBy(u => u.YONETICI_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.YONETICIs.Where(k => k.YONETICI_ADI.Contains(arama)).Count());
                liste = db.YONETICIs.Where(k => k.YONETICI_ADI.Contains(arama)).OrderBy(u => u.YONETICI_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            ViewData["veri"] = arama;
            ViewData["aktifsayfa"] = aktifsayfa;
            return View(liste);
        }

        public void Sayfalama(int satirsayisi)
        {
            int toplamsatir = satirsayisi;
            int toplamsayfa = toplamsatir / sayfadakisatirsayisi;
            if (toplamsatir % sayfadakisatirsayisi != 0)
            {
                toplamsayfa++;
            }
            ViewData["toplamsatir"] = toplamsatir;
            ViewData["toplamsayfa"] = toplamsayfa;
        }

        // GET: Yoneticiler/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YONETICI yONETICI = db.YONETICIs.Find(id);
            if (yONETICI == null)
            {
                return HttpNotFound();
            }
            return View(yONETICI);
        }

        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Create(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            if (id == null)
            {
                YONETICI k1 = new YONETICI();
                return View(k1);
            }
            YONETICI k = db.YONETICIs.Find(id);
            var liste = db.YETKI_GRUBU.ToList();
            ViewData["yetkigrubu"] = liste;//hersey object olarak saklanır.
            return View(k);
        }

        [HttpPost]//Kayıt işlemi
        [ValidateAntiForgeryToken]
        public ActionResult Create(YONETICI k)
        {
            if (ModelState.IsValid)
            {
                if (k.YONETICI_REFNO == 0)
                {
                    db.YONETICIs.Add(k);
                }
                else
                {
                    db.Entry(k).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var liste = db.YETKI_GRUBU.ToList();
            ViewData["yetkigrubu"] = liste;//hersey object olarak saklanır.
            return View(k);
        }

        // GET: Yoneticiler/Edit/5
        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Edit(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            if (id == null)
            {
                YONETICI k1 = new YONETICI();
                return View(k1);
            }
            YONETICI k = db.YONETICIs.Find(id);
            var liste = db.YETKI_GRUBU.ToList();
            ViewData["yetkigrubu"] = liste;//hersey object olarak saklanır.
            return View(k);
        }

        [HttpPost]//Kayıt işlemi
        [ValidateAntiForgeryToken]
        public ActionResult Edit(YONETICI k)
        {
            if (ModelState.IsValid)
            {
                if (k.YONETICI_REFNO == 0)
                {
                    db.YONETICIs.Add(k);
                }
                else
                {
                    db.Entry(k).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var liste = db.YETKI_GRUBU.ToList();
            ViewData["yetkigrubu"] = liste;//hersey object olarak saklanır.
            return View(k);
        }

        // GET: Yoneticiler/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YONETICI yONETICI = db.YONETICIs.Find(id);
            if (yONETICI == null)
            {
                return HttpNotFound();
            }
            return View(yONETICI);
        }

        // POST: Yoneticiler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            YONETICI yONETICI = db.YONETICIs.Find(id);
            db.YONETICIs.Remove(yONETICI);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Index", "Yoneticiler", new { arama = txtAra });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void ExportToExcel()       //excele çıkartma
        {

            List<YONETICI> emplist = db.YONETICIs.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Yöneticiler Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "YoneticiRefno";
            ws.Cells["B6"].Value = "AdıSoyadı";
            ws.Cells["C6"].Value = "Parola";
            ws.Cells["D6"].Value = "Durumu";
            ws.Cells["E6"].Value = "YetkiGrubu";
            ws.Cells["F6"].Value = "Email";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.YONETICI_REFNO <= db.YONETICIs.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));

                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.YONETICI_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.YONETICI_ADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.PAROLA;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.DURUMU;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.YETKI_GRUBU.GRUP_ADI;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.EMAIL;

                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "ExcelReport.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

        }

        //PDF e basarken tüm listeyi alıyor.
        public ActionResult GetAll()
        {
            var uListe = db.YONETICIs.ToList();
            return View(uListe);
        }
        //PDF basarken tüm listeyi basıyor.
        public ActionResult PrintAll()
        {
            var q = new ActionAsPdf("GetAll");
            return q;
        }
    }
}

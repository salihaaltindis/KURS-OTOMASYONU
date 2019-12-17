using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using KURSOTOMASYON.Models;
using OfficeOpenXml;
using Rotativa;

namespace KURSOTOMASYON.Controllers
{
    [Authorize]
    public class KatilmaDurumuController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;

        // GET: KatilmaDurumu

        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "KatilmaDurumu").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");


            List<KATILMA_DURUMU> liste = new List<KATILMA_DURUMU>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.KATILMA_DURUMU.Count());
                liste = db.KATILMA_DURUMU.OrderBy(u => u.KATILMA_DURUMU_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.KATILMA_DURUMU.Where(k => k.KATILMA_DURUMU1.Contains(arama)).Count());
                liste = db.KATILMA_DURUMU.Where(k => k.KATILMA_DURUMU1.Contains(arama)).OrderBy(u => u.KATILMA_DURUMU_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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


        public ActionResult Delete(int id)
        {
            try
            {
                KATILMA_DURUMU k = db.KATILMA_DURUMU.Find(id);
                if (k != null)
                {
                    db.KATILMA_DURUMU.Remove(k);
                    db.SaveChanges();
                }
            }
            catch (Exception EX)
            {
                ViewBag.Message = "SİLERKEN HATA OLUŞTU!";
                return RedirectToAction("Index", "Hata", new { mesaj = ViewBag.Message });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Create(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            if (id == null)
            {
                KATILMA_DURUMU k1 = new KATILMA_DURUMU();
                return View(k1);
            }
            KATILMA_DURUMU k = db.KATILMA_DURUMU.Find(id);
            return View(k);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        public ActionResult Create(KATILMA_DURUMU k)
        {
            if (ModelState.IsValid)
            {
                if (k.KATILMA_DURUMU_REFNO == 0)
                {
                    db.KATILMA_DURUMU.Add(k);
                }
                else
                {
                    db.Entry(k).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(k);
        }

        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Index", "KatilmaDurumu", new { arama = txtAra });
        }
        public void ExportToExcel()       //excele çıkartma
        {

            List<KATILMA_DURUMU> emplist = db.KATILMA_DURUMU.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Katılma Durumu Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "KatılmaDurumuRefno";
            ws.Cells["B6"].Value = "KatılmaDurumu";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.KATILMA_DURUMU_REFNO <= db.KATILMA_DURUMU.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.KATILMA_DURUMU_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.KATILMA_DURUMU1;

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
            var uListe = db.KATILMA_DURUMU.ToList();
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

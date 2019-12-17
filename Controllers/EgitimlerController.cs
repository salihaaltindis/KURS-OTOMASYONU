using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KURSOTOMASYON.Models;
using OfficeOpenXml;
using Rotativa;

namespace KURSOTOMASYON.Controllers
{
    [Authorize]
    public class EgitimlerController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;

        // GET: Egitimler
        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "Egitimler").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");


            List<VW_EGITIM_KATEGORI> liste = new List<VW_EGITIM_KATEGORI>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.VW_EGITIM_KATEGORI.Count());
                liste = db.VW_EGITIM_KATEGORI.OrderBy(u => u.EGITIM_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.VW_EGITIM_KATEGORI.Where(k => k.EGITIM_ADI.Contains(arama)).Count());
                liste = db.VW_EGITIM_KATEGORI.Where(k => k.EGITIM_ADI.Contains(arama)).OrderBy(u => u.EGITIM_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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
            EGITIM e = db.EGITIMs.Find(id);
            try
            {
                if (e != null)
                {
                    db.EGITIMs.Remove(e);
                    db.SaveChanges();
                }
            }
            catch (Exception EX)
            {
                ViewBag.Message = "SİLERKEN HATA OLUŞTU BU EĞİTİME BAĞIMLI KATEGORİ VAR!";
                return RedirectToAction("Index", "Hata", new { mesaj = ViewBag.Message });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Create(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            EGITIM egitim = new EGITIM();
            if (id != null)//guncelleme yapılacak
            {
                egitim = db.EGITIMs.Find(id);
                if (egitim == null)
                {
                    egitim = new EGITIM();
                }
            }
            var liste = db.KATEGORIs.ToList();
            ViewData["kategori"] = liste;//hersey object olarak saklanır.
            return View(egitim);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        public ActionResult Create(EGITIM e)
        {
            if (ModelState.IsValid)
            {
                if (e.EGITIM_REFNO == 0)
                {
                    db.EGITIMs.Add(e);
                }
                else
                {
                    db.Entry(e).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var liste = db.KATEGORIs.ToList();
            ViewData["kategori"] = liste;//hersey object olarak saklanır.
            return View(e);
        }

        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Index", "Egitimler", new { arama = txtAra });
        }

        public void ExportToExcel()       //excele çıkartma
        {

            List<EGITIM> emplist = db.EGITIMs.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Eğitimler Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "EğitimRefno";
            ws.Cells["B6"].Value = "EğitimAdı";
            ws.Cells["C6"].Value = "KategoriAdı";
            ws.Cells["D6"].Value = "İçerik";
            ws.Cells["E6"].Value = "Ücret";
            ws.Cells["F6"].Value = "Saat";
            ws.Cells["G6"].Value = "Durumu";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.EGITIM_REFNO <= db.EGITIMs.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.EGITIM_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.EGITIM_ADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.KATEGORI.KATEGORI_ADI;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ICERIK;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.UCRET;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.SAAT;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.DURUMU;

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
            var uListe = db.EGITIMs.ToList();
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
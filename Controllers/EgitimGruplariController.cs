using KURSOTOMASYON.Models;
using OfficeOpenXml;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KURSOTOMASYON.Controllers
{
    [Authorize]
    public class EgitimGruplariController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;

        // GET: EgitimGruplari
        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "EgitimGruplari").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");



            List<VW_EGITIM_GRUP_EGITMEN_EGITIM> liste = new List<VW_EGITIM_GRUP_EGITMEN_EGITIM>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.VW_EGITIM_GRUP_EGITMEN_EGITIM.Count());
                liste = db.VW_EGITIM_GRUP_EGITMEN_EGITIM.OrderByDescending(u => u.BASLANGIC_TARIH).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.VW_EGITIM_GRUP_EGITMEN_EGITIM.Where(k => k.EGITIM_GRUP_ADI.Contains(arama)).Count());
                //EN SON ACILAN KURSTAN İTİBAREN GRUPLARI BASIYOR.
                liste = db.VW_EGITIM_GRUP_EGITMEN_EGITIM.Where(k => k.EGITIM_GRUP_ADI.Contains(arama)).OrderByDescending(u => u.BASLANGIC_TARIH).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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
            EGITIM_GRUP e = db.EGITIM_GRUP.Find(id);
            try
            {
                if (e != null)
                {
                    db.EGITIM_GRUP.Remove(e);
                    db.SaveChanges();
                }
            }
            catch (Exception EX)
            {
                ViewBag.Message = "SİLERKEN HATA OLUŞTU BU EĞİTİM GRUBUNDA AKTİF EĞİTMEN VE EĞİTİMLER VAR!";
                return RedirectToAction("Index", "Hata", new { mesaj = ViewBag.Message });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Create(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            EGITIM_GRUP egitimgrup = new EGITIM_GRUP();
            if (id != null)//guncelleme yapılacak
            {
                egitimgrup = db.EGITIM_GRUP.Find(id);
                if (egitimgrup == null)
                {
                    egitimgrup = new EGITIM_GRUP();
                }

            }
            var egitimliste = db.EGITIMs.Where(u=>u.DURUMU == true).ToList();   ////durumu false olan eğitimler listelenmiyor.
            ViewData["egitim"] = egitimliste;//hersey object olarak saklanır.
            var egitmenliste = db.EGITMEN.Where(u => u.DURUM == true).ToList();  ////durumu false olan eğitmenler listelenmiyor.
            ViewData["egitmen"] = egitmenliste;
            return View(egitimgrup);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        public ActionResult Create(EGITIM_GRUP e)
        {
            if (ModelState.IsValid)
            {
                if (e.EGITIM_GRUP_REFNO == 0)
                {
                    db.EGITIM_GRUP.Add(e);
                }
                else
                {
                    db.Entry(e).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var egitimliste = db.EGITIMs.ToList();
            ViewData["egitim"] = egitimliste;//hersey object olarak saklanır.
            var egitmenliste = db.EGITMEN.ToList();
            ViewData["egitmen"] = egitmenliste;
            return View(e);
        }

        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Index", "EgitimGruplari", new { arama = txtAra });
        }
        public void ExportToExcel()       //excele çıkartma
        {

            List<EGITIM_GRUP> emplist = db.EGITIM_GRUP.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");



            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Eğitim Grupları Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "EğitimGrupRefno";
            ws.Cells["B6"].Value = "EğitimGrupAdı";
            ws.Cells["C6"].Value = "EğitimAdI";
            ws.Cells["D6"].Value = "EğitmenAdıSoyadı";
            ws.Cells["E6"].Value = "Ücret";
            ws.Cells["F6"].Value = "BaşlangıçTarih";
            ws.Cells["G6"].Value = "BitişTarih";
            ws.Cells["H6"].Value = "BaşlangıçSaat";
            ws.Cells["I6"].Value = "BitişSaat";
            ws.Cells["J6"].Value = "Kontenjan";
            ws.Cells["K6"].Value = "Günler";
            ws.Cells["L6"].Value = "Doluluk";
            ws.Cells["M6"].Value = "Ayrılan";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.EGITIM_GRUP_REFNO <= db.EGITIM_GRUP.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.EGITIM_GRUP_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.EGITIM_GRUP_ADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.EGITIM.EGITIM_ADI;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.EGITMan.ADI_SOYADI;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.UCRET;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.BASLANGIC_TARIH;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.BITIS_TARIH;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.BASLANGIC_SAAT;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.BITIS_SAAT;
                ws.Cells[string.Format("J{0}", rowStart)].Value = item.KONTENJAN;
                ws.Cells[string.Format("K{0}", rowStart)].Value = item.GUNLER;
                ws.Cells[string.Format("L{0}", rowStart)].Value = item.DOLULUK;
                ws.Cells[string.Format("M{0}", rowStart)].Value = item.AYRILAN;

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
            var uListe = db.EGITIM_GRUP.ToList();
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
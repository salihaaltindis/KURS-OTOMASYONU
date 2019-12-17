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
    public class OnKayitlarController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;
        // GET: OnKayitlar
        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "OnKayitlar").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");


            List<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU> liste = new List<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU.Count());
                //gorusme tarihine gore sıralayacak
                liste = db.VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU.OrderByDescending(u => u.GORUSME_TARIHI).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU.Where(k => k.ADI_SOYADI.Contains(arama)).Count());
                liste = db.VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU.Where(k => k.ADI_SOYADI.Contains(arama)).OrderBy(u => u.ON_KAYIT_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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
            ON_KAYIT o = db.ON_KAYIT.Find(id);
            try
            {
                if (o != null)
                {
                    db.ON_KAYIT.Remove(o);
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
            ON_KAYIT onkayit = new ON_KAYIT();
            if (id != null)//guncelleme yapılacak
            {
                onkayit = db.ON_KAYIT.Find(id);
                if (onkayit == null)
                {
                    onkayit = new ON_KAYIT();
                }

            }
            var egitimgrupadlari = db.EGITIM_GRUP.ToList();
            ViewData["egitimgrupadlari"] = egitimgrupadlari;
            var katilmadurumu = db.KATILMA_DURUMU.ToList();
            ViewData["katilmadurumu"] = katilmadurumu;
            return View(onkayit);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        public ActionResult Create(ON_KAYIT o)
        {
            if (ModelState.IsValid)
            {
                if (o.ON_KAYIT_REFNO == 0)
                {
                    db.ON_KAYIT.Add(o);
                }
                else
                {
                    db.Entry(o).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var egitimgrupadlari = db.EGITIM_GRUP.ToList();
            ViewData["egitimgrupadlari"] = egitimgrupadlari;
            var katilmadurumu = db.KATILMA_DURUMU.ToList();
            ViewData["katilmadurumu"] = katilmadurumu;
            return View(o);
        }

        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Index", "OnKayitlar", new { arama = txtAra });
        }
        public void ExportToExcel()       //excele çıkartma
        {

            List<ON_KAYIT> emplist = db.ON_KAYIT.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Ön Kayıtlar Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "ÖnKayıtRefno";
            ws.Cells["B6"].Value = "EğitimGrupAdı";
            ws.Cells["C6"].Value = "AdıSoyadı";
            ws.Cells["D6"].Value = "Telefon";
            ws.Cells["E6"].Value = "Adres";
            ws.Cells["F6"].Value = "Email";
            ws.Cells["G6"].Value = "Açıklama";
            ws.Cells["H6"].Value = "GörüşmeTarihi";
            ws.Cells["I6"].Value = "KatılmaDurumu";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.ON_KAYIT_REFNO <= db.ON_KAYIT.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.ON_KAYIT_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.EGITIM_GRUP.EGITIM_GRUP_ADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.ADI_SOYADI;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.TELEFON;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.ADRES;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.EMAIL;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.ACIKLAMA;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.GORUSME_TARIHI;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.KATILMA_DURUMU.KATILMA_DURUMU1;


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
            var uListe = db.ON_KAYIT.ToList();
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
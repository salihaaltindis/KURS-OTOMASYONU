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

    public class EgitmenHareketlerController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 10;

        // GET: EğitimHareketler

        public ActionResult Index(int id)
        {
            ViewData["id"] = id;
            List<VW_EGITMEN_HAREKET_EGITMEN> liste = new List<VW_EGITMEN_HAREKET_EGITMEN>();
            if (liste != null)
            {
                liste = db.VW_EGITMEN_HAREKET_EGITMEN.Where(k => k.EGITMEN_REFNO == id).ToList();

                return View(liste);

            }

            return View(liste);
        }




        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Create(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            EGITMEN_HAREKET egitmenhareket = new EGITMEN_HAREKET();
            
            if (id != null)//guncelleme yapılacak
            {
                egitmenhareket = db.EGITMEN_HAREKET.Find(id);
                if (egitmenhareket == null)
                {
                    egitmenhareket = new EGITMEN_HAREKET();
                    ViewData["id"]=egitmenhareket.EGITMEN_REFNO;
                }
            }
            ViewData["id"] = egitmenhareket.EGITMEN_REFNO;
            var egitmentc = db.EGITMEN.ToList();
            ViewData["egitmentc"] = egitmentc;
            
            return View(egitmenhareket);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        public ActionResult Create(EGITMEN_HAREKET e)
        {
            var egitmentc = db.EGITMEN.ToList();
            ViewData["egitmentc"] = egitmentc;

            if (ModelState.IsValid)
            {
                if (e.EGITMEN_HAREKET_REFNO == 0)
                {
                    db.EGITMEN_HAREKET.Add(e);
                    
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(e).State = System.Data.Entity.EntityState.Modified;
                    
                }
                
                db.SaveChanges();
                //return RedirectToAction("Index");
            return RedirectToAction("Index", "EgitmenHareketler", new { id = e.EGITMEN_REFNO });
        }

        
            return View(e);
        }

        public ActionResult Listele(string arama, int aktifsayfa = 0)
        {

            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "EgitmenHareketler").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");




            List<VW_EGITMEN_HAREKET_EGITMEN> liste = new List<VW_EGITMEN_HAREKET_EGITMEN>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.VW_EGITMEN_HAREKET_EGITMEN.Count());
                liste = db.VW_EGITMEN_HAREKET_EGITMEN.OrderBy(u => u.EGITMEN_HAREKET_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.VW_EGITMEN_HAREKET_EGITMEN.Where(k => k.ADI_SOYADI.Contains(arama)).Count());
                liste = db.VW_EGITMEN_HAREKET_EGITMEN.Where(k => k.ADI_SOYADI.Contains(arama)).OrderBy(u => u.EGITMEN_HAREKET_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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

        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Listele", "EgitmenHareketler", new { arama = txtAra });
        }



        public ActionResult Delete(int id)
        {
            EGITMEN_HAREKET k = db.EGITMEN_HAREKET.Find(id);
            try
            {
                if (k != null)
                {
                    db.EGITMEN_HAREKET.Remove(k);
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
        public void ExportToExcel()       //excele çıkartma
        {

            List<VW_EGITMEN_HAREKET_EGITMEN> emplist = db.VW_EGITMEN_HAREKET_EGITMEN.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Eğitmen Hareketleri Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "EğitmenHareketRefno";
            ws.Cells["B6"].Value = "EğitmenAdıSoyadı";
            ws.Cells["C6"].Value = "TC";
            ws.Cells["D6"].Value = "Durum";
            ws.Cells["E6"].Value = "Tarih";
            ws.Cells["F6"].Value = "Alacak";
            ws.Cells["G6"].Value = "Ödenen";
            ws.Cells["H6"].Value = "Açıklama";
              

            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.EGITMEN_HAREKET_REFNO <= db.VW_EGITMEN_HAREKET_EGITMEN.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.EGITMEN_HAREKET_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.ADI_SOYADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.TC;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.DURUM;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.TARIH;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.ALACAK;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.ODENEN;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.ACIKLAMA;

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
            var uListe = db.EGITMEN_HAREKET.ToList();
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

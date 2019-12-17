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
    public class KursiyerlerController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;
        // GET: Kursiyerler
        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "Kursiyerler").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");


            List<KURSIYER> liste = new List<KURSIYER>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.KURSIYERs.Count());
                liste = db.KURSIYERs.OrderBy(u => u.KURSIYER_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.KURSIYERs.Where(k => k.TC.Contains(arama)).Count());
                liste = db.KURSIYERs.Where(k => k.TC.Contains(arama)).OrderBy(u => u.KURSIYER_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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
            KURSIYER k = db.KURSIYERs.Find(id);
            try
            {
                if (k != null)
                {
                    db.KURSIYERs.Remove(k);
                    db.SaveChanges();
                }
            }
            catch (Exception EX)
            {
                ViewBag.Message = "SİLERKEN HATA OLUŞTU BU KİŞİNİN BAĞLI OLDUĞU HAREKET DÖKÜMLERİ VAR!";
                return RedirectToAction("Index", "Hata", new { mesaj = ViewBag.Message });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Create(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            KURSIYER kursiyer = new KURSIYER();
            if (id != null)//guncelleme yapılacak
            {
                kursiyer = db.KURSIYERs.Find(id);
                if (kursiyer == null)
                {
                    kursiyer = new KURSIYER();
                }
            }
            return View(kursiyer);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        public ActionResult Create(KURSIYER k)
        {
            if (ModelState.IsValid)
            {
                if (k.KURSIYER_REFNO == 0)
                {
                    db.KURSIYERs.Add(k);
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
            return RedirectToAction("Index", "Kursiyerler", new { arama = txtAra });
        }

        public void ExportToExcel()       //excele çıkartma
        {

            List<KURSIYER> emplist = db.KURSIYERs.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Kursiyerler Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "KursiyerRefno";
            ws.Cells["B6"].Value = "AdıSoyadı";
            ws.Cells["C6"].Value = "TC";
            ws.Cells["D6"].Value = "Adres";
            ws.Cells["E6"].Value = "Telefon";
            ws.Cells["F6"].Value = "Parola";
            ws.Cells["G6"].Value = "Email";
            ws.Cells["H6"].Value = "Cinsiyet";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.KURSIYER_REFNO <= db.KURSIYERs.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.KURSIYER_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.ADI_SOYADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.TC;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ADRES;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.TELEFON;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.PAROLA;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.EMAIL;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.CINSIYET;

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
            var uListe = db.KURSIYERs.ToList();
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
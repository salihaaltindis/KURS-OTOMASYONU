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
using Microsoft.Reporting.WebForms;
using OfficeOpenXml;
using Rotativa;

namespace KURSOTOMASYON.Controllers
{
    [Authorize]
    public class KategorilerController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;

        // GET: Kategoriler
        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "Kategoriler").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");


            List<KATEGORI> liste = new List<KATEGORI>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.KATEGORIs.Count());
                liste = db.KATEGORIs.OrderBy(u => u.KATEGORI_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.KATEGORIs.Where(k => k.KATEGORI_ADI.Contains(arama)).Count());
                liste = db.KATEGORIs.Where(k => k.KATEGORI_ADI.Contains(arama)).OrderBy(u => u.KATEGORI_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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
            KATEGORI k = db.KATEGORIs.Find(id);
            try
            {
                if (k != null)
                {
                    db.KATEGORIs.Remove(k);
                    db.SaveChanges();
                }
            }
            catch (Exception EX)
            {
                ViewBag.Message = "SİLERKEN HATA OLUŞTU BU KATEGORİYE BAĞIMLI EĞİTİMLER VAR!";
                return RedirectToAction("Index","Hata", new { mesaj= ViewBag.Message });
            }     
            return RedirectToAction("Index");
        }

        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Create(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            if (id == null)
            {
                KATEGORI k1 = new KATEGORI();
                return View(k1);
            }
            KATEGORI k = db.KATEGORIs.Find(id);
            return View(k);
        }

        [HttpPost]//Kayıt işlemi
        [ValidateAntiForgeryToken]
        public ActionResult Create(KATEGORI k)
        {
            if (ModelState.IsValid)
            {
                if (k.KATEGORI_REFNO == 0)
                {
                    db.KATEGORIs.Add(k);
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
            //return View("Index",liste);//View e yönlendirir
            return RedirectToAction("Index", "Kategoriler", new { arama = txtAra });
        }


        //public ActionResult Reporting(string ReportType)
        //{
        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath("~/Reporting/Kategori.rdlc");
        //    ReportDataSource reportDataSource = new ReportDataSource();
        //    reportDataSource.Name = "KategoriDataSet";
        //    reportDataSource.Value = db.KATEGORIs.ToList();
        //    localReport.DataSources.Add(reportDataSource);
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension ;
        //    if (ReportType == "Excel")
        //    {
        //        fileNameExtension = "xlsx";
        //    }
        //    else if (ReportType == "PDF")
        //    {
        //        fileNameExtension = "pdf";
        //    }
        //    string[] streams;
        //    Warning[] warnings;
        //    byte[] renderedByte;
        //    renderedByte = localReport.Render(ReportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        //    Response.AddHeader("content-disposition","attachment:filename= kategori_report." +fileNameExtension);
        //    return File(renderedByte, fileNameExtension);
        //}

        
        public void ExportToExcel()       //excele çıkartma
        {
            
            List<KATEGORI> emplist = db.KATEGORIs.ToList();
            
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Kategoriler Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "KategoriRefno";
            ws.Cells["B6"].Value = "KategoriAdı";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.KATEGORI_REFNO <= db.KATEGORIs.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.KATEGORI_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.KATEGORI_ADI;

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
            var uListe = db.KATEGORIs.ToList();
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

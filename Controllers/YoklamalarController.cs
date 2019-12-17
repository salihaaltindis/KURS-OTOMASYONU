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
    public class YoklamalarController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;

        // GET: Yoklamalar

        //public ActionResult Index()
        //{
        //    var liste = db.EGITIM_GRUP.ToList();
        //    return View(liste);
        //}

        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);

            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;

            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "Yoklamalar").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");

            List<EGITIM_GRUP> liste = new List<EGITIM_GRUP>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.EGITIM_GRUP.Count());
                liste = db.EGITIM_GRUP.OrderBy(u => u.EGITIM_GRUP_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.EGITIM_GRUP.Where(k => k.EGITIM_GRUP_ADI.Contains(arama)).Count());
                liste = db.EGITIM_GRUP.Where(k => k.EGITIM_GRUP_ADI.Contains(arama)).OrderBy(u => u.EGITIM_GRUP_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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
            return RedirectToAction("Index", "Yoklamalar", new { arama = txtAra });
        }

        public ActionResult Yoklamalar(int id)//egitim grubu refno
        {
            ViewData["id"] = id;
            ViewData["kayitlar"] = db.KAYITs.ToList();//kayıt ref id
            //var TARIH = Request.Form["TARIH"];
           
            var liste = db.YOKLAMAs.Where(y => y.EGITIM_GRUP_REFNO == id).ToList();
       
            return View(liste);
        }

        public ActionResult Kayit(FormCollection formdata)
        {
            //eski yoklamaları silmeliyiz.
            int EGITIM_GRUBU_REFNO = Convert.ToInt32(formdata["EGITIM_GRUBU_REFNO"]);
            var yoklamalar = db.YOKLAMAs.Where(y => y.EGITIM_GRUP_REFNO == EGITIM_GRUBU_REFNO);
            db.YOKLAMAs.RemoveRange(yoklamalar);
            db.SaveChanges();

            int a = db.KAYITs.Where(k => k.EGITIM_GRUP_REFNO == EGITIM_GRUBU_REFNO).Count();
            //int toplamsatir = db.KAYITs.Count();
            
            for (int i = 0; i < a; i++)
            {
                int kayitrefno = db.KAYITs.Where(k => k.EGITIM_GRUP_REFNO == EGITIM_GRUBU_REFNO).ToList()[i].KAYIT_REFNO;
                int kursiyerref = db.KAYITs.Where(k => k.EGITIM_GRUP_REFNO == EGITIM_GRUBU_REFNO).ToList()[i].KURSIYER_REFNO;

                bool DURUMU = false;
                if (formdata["DURUMU" + kayitrefno] != null) DURUMU = true;
                var ACIKLAMA1 = formdata["ACIKLAMA" + kayitrefno];


                YOKLAMA y = new YOKLAMA()
                {
                    KAYIT_REFNO = kayitrefno,
                    KURSIYER_REFNO = kursiyerref,
                    ACIKLAMA = ACIKLAMA1,
                    EGITIM_GRUP_REFNO = EGITIM_GRUBU_REFNO,
                    DURUMU = DURUMU,
                };
                db.YOKLAMAs.Add(y);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public void ExportToExcel()       //excele çıkartma
        {

            List<VW_YOKLAMA_KURSIYER> emplist = db.VW_YOKLAMA_KURSIYER.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Yoklamalar Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "YoklamaRefno";
            ws.Cells["B6"].Value = "AdıSoyadı";
            ws.Cells["C6"].Value = "Tarih";
            ws.Cells["D6"].Value = "Durumu";
            ws.Cells["E6"].Value = "Açıklama";
            ws.Cells["F6"].Value = "EğitimGrupAdı";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.YOKLAMA_REFNO <= db.YOKLAMAs.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.YOKLAMA_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.ADI_SOYADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.TARIH;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.DURUMU;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.ACIKLAMA;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.EGITIM_GRUP_ADI;

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
            var uListe = db.YOKLAMAs.ToList();
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
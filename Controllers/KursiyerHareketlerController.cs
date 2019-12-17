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

    public class KursiyerHareketlerController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 10;

        // GET: KursiyerHareketler
        public ActionResult Index(int id)
        {
            ViewData["id"] = id;
            List<VW_KURSIYER_HAREKET_KAYIT_KURSIYER> liste = new List<VW_KURSIYER_HAREKET_KAYIT_KURSIYER>();
            if (liste != null)
            {
                liste = db.VW_KURSIYER_HAREKET_KAYIT_KURSIYER.Where(k => k.KAYIT_REFNO == id).ToList();

                return View(liste);

            }

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

        public ActionResult HareketOlustur(int kayıtrefno, int tutar)
        {
            List<KURSIYER_HAREKET> liste = db.KURSIYER_HAREKET.Where(a => a.KAYIT_REFNO == kayıtrefno && a.BORC > a.ODENEN).ToList();

            foreach (var t in liste)
            {
                int borc = t.BORC;
                int odenen = t.ODENEN;
                if (tutar > (borc - odenen))
                {
                    t.ODENEN = t.BORC;

                    tutar = tutar - (borc - odenen);

                }
                else
                {
                    t.ODENEN = odenen + tutar;
                    tutar = 0;
                    break;
                }

            }
            db.SaveChanges();
            return RedirectToAction("Index", new { id = kayıtrefno });
        }

        public ActionResult Listele(string arama, int aktifsayfa = 0)
        {

            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "KursiyerHareketler").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");



            List<VW_KURSIYER_HAREKET_KAYIT_KURSIYER> liste = new List<VW_KURSIYER_HAREKET_KAYIT_KURSIYER>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.VW_KURSIYER_HAREKET_KAYIT_KURSIYER.Count());
                liste = db.VW_KURSIYER_HAREKET_KAYIT_KURSIYER.OrderBy(u => u.KURSIYER_HAREKET_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.VW_KURSIYER_HAREKET_KAYIT_KURSIYER.Where(k => k.ADI_SOYADI.Contains(arama)).Count());
                liste = db.VW_KURSIYER_HAREKET_KAYIT_KURSIYER.Where(k => k.ADI_SOYADI.Contains(arama)).OrderBy(u => u.KURSIYER_HAREKET_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            ViewData["veri"] = arama;
            ViewData["aktifsayfa"] = aktifsayfa;

            return View(liste);
        }


        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Listele", "KursiyerHareketler", new { arama = txtAra });
        }

        public ActionResult Delete(int id)
        {
            KURSIYER_HAREKET k = db.KURSIYER_HAREKET.Find(id);
            try
            {
                if (k != null)
                {
                    db.KURSIYER_HAREKET.Remove(k);
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

            List<VW_KURSIYER_HAREKET_KAYIT_KURSIYER> emplist = db.VW_KURSIYER_HAREKET_KAYIT_KURSIYER.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Kursiyer Hareketleri Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "KursiyerHareketRefno";
            ws.Cells["B6"].Value = "KursiyerAdıSoyadı";
            ws.Cells["C6"].Value = "KayıtDurumu";
            ws.Cells["D6"].Value = "Tarih";
            ws.Cells["E6"].Value = "Borç";
            ws.Cells["F6"].Value = "Ödenen";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.KURSIYER_HAREKET_REFNO <= db.VW_KURSIYER_HAREKET_KAYIT_KURSIYER.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.KURSIYER_HAREKET_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.ADI_SOYADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.KAYIT_DURUMU;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.TARIH;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.BORC;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.ODENEN;

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
            var uListe = db.KURSIYER_HAREKET.ToList();
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
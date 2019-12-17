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
    public class KayitlarController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;

        // GET: Kayitlar
        public ActionResult Index(string arama, int aktifsayfa = 0)
        {
            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "Kayitlar").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");



            List<VW_KAYIT_KURSIYER_EGITIMGRUP> liste = new List<VW_KAYIT_KURSIYER_EGITIMGRUP>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.VW_KAYIT_KURSIYER_EGITIMGRUP.Count());
                liste = db.VW_KAYIT_KURSIYER_EGITIMGRUP.OrderBy(u => u.KAYIT_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.VW_KAYIT_KURSIYER_EGITIMGRUP.Where(k => k.ADI_SOYADI.Contains(arama)).Count());
                liste = db.VW_KAYIT_KURSIYER_EGITIMGRUP.Where(k => k.ADI_SOYADI.Contains(arama)).OrderBy(u => u.KAYIT_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
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
            KAYIT k = db.KAYITs.Find(id);
            try
            {
                if (k != null)
                {
                    db.KAYITs.Remove(k);
                    db.SaveChanges();
                    EgitimdenKisiDus(k.EGITIM_GRUP_REFNO);
                }
            }
            catch (Exception EX)
            {
                ViewBag.Message = "SİLERKEN HATA OLUŞTU BU KAYITA BAĞLI KURSİYER VAR!";
                return RedirectToAction("Index", "Hata", new { mesaj = ViewBag.Message });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Create(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            KAYIT kayit = new KAYIT();
            if (id != null)//guncelleme yapılacak
            {
                kayit = db.KAYITs.Find(id);
                if (kayit == null)
                {
                    kayit = new KAYIT();
                }
            }
            var egitimgrup = db.EGITIM_GRUP.Where(k=>k.KONTENJAN>k.DOLULUK).ToList();
            ViewData["egitimgrup"] = egitimgrup;
            var kursiyer = db.KURSIYERs.ToList();
            ViewData["kursiyer"] = kursiyer;
            return View(kayit);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        public ActionResult Create(KAYIT k)
        {
            if (ModelState.IsValid)
            {
                if (k.KAYIT_REFNO == 0)
                {
                    db.KAYITs.Add(k);

                    var pesinat = Request.Form["PESINAT"];
                    var p = Convert.ToInt32(pesinat);
                    var pesinattarih = Request.Form["PESINATBASLANGICTARIH"];
                    var pt = Convert.ToDateTime(pesinattarih);
                    db.SaveChanges();


                    EgitimeKisiEkle(k.EGITIM_GRUP_REFNO);
                    Taksitlendir(k.EGITIM_GRUP_REFNO,k.KAYIT_UCRETI,k.TAKSIT_SAYISI,k.KAYIT_REFNO,p,pt);
                    HareketOlustur(k.KAYIT_UCRETI, k.TAKSIT_SAYISI, k.KAYIT_REFNO, p, pt);

                }
                else
                {
                    db.Entry(k).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
               
                return RedirectToAction("Index");
            }
            var egitimgrup = db.EGITIM_GRUP.Where(a => a.KONTENJAN > a.DOLULUK).ToList();
            ViewData["egitimgrup"] = egitimgrup;
            var kursiyer = db.KURSIYERs.ToList();
            var PESINAT = db.KAYITs.ToList();
            ViewData["PESINAT"] = PESINAT;
            ViewData["kursiyer"] = kursiyer;
            
            return View(k);
        }

        [HttpGet]//yeni ve guncelleme işlemi
        public ActionResult Edit(int? id)//bu gosterim id null olabilir. hem create hem edit sayflarını ortak yaptıgımız için id yolladıgımızda yollamadıgımızda olacak.
        {
            KAYIT kayit = new KAYIT();
            if (id != null)//guncelleme yapılacak
            {
                kayit = db.KAYITs.Find(id);
                if (kayit == null)
                {
                    kayit = new KAYIT();
                }
            }
            var egitimgrup = db.EGITIM_GRUP.Where(k => k.KONTENJAN > k.DOLULUK).ToList();
            ViewData["egitimgrup"] = egitimgrup;
            var kursiyer = db.KURSIYERs.ToList();
            ViewData["kursiyer"] = kursiyer;
            return View(kayit);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KAYIT k)
        {
            if (ModelState.IsValid)
            {
                if (k.KAYIT_REFNO == 0)
                {
                    db.KAYITs.Add(k);

                    //var pesinat = Request.Form["PESINAT"];
                    //var p = Convert.ToInt32(pesinat);
                    //var pesinattarih = Request.Form["PESINATBASLANGICTARIH"];
                    //var pt = Convert.ToDateTime(pesinattarih);
                    db.SaveChanges();


                    //EgitimeKisiEkle(k.EGITIM_GRUP_REFNO);
                    //Taksitlendir(k.EGITIM_GRUP_REFNO, k.KAYIT_UCRETI, k.TAKSIT_SAYISI, k.KAYIT_REFNO, p, pt);
                    //HareketOlustur(k.KAYIT_UCRETI, k.TAKSIT_SAYISI, k.KAYIT_REFNO, p, pt);

                }
                else
                {
                    db.Entry(k).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            var egitimgrup = db.EGITIM_GRUP.Where(a => a.KONTENJAN > a.DOLULUK).ToList();
            ViewData["egitimgrup"] = egitimgrup;
            //var kursiyer = db.KURSIYERs.ToList();
            //var PESINAT = db.KAYITs.ToList();
            //ViewData["PESINAT"] = PESINAT;
            //ViewData["kursiyer"] = kursiyer;

            return View(k);
        }

        //Kursiyer hareket oluşturma
        public void HareketOlustur(int ucret, int taksitsayisi, int kayitref, int PESINAT, DateTime PESINATBASLANGICTARIH)
        {
            ucret = ucret - PESINAT;
            int aylik_tutar = ucret / taksitsayisi;

            for (int i = 0; i < taksitsayisi; i++)
            {
                KURSIYER_HAREKET kh = new KURSIYER_HAREKET();
                kh.KAYIT_REFNO = kayitref;
                kh.BORC = aylik_tutar;
                kh.ODENEN = 0;
                //t.TARIH = DateTime.Today.AddMonths(i);// ay 1 artır ***************************************
                kh.TARIH = PESINATBASLANGICTARIH.AddMonths(i);

                db.KURSIYER_HAREKET.Add(kh);
            }
            db.SaveChanges();
        }

        //Kursiyer için olusturulan kaydı taksitlendirme
        public void Taksitlendir(int grupREFNO,int ucret,int taksitsayisi,int kayitref,int PESINAT,DateTime PESINATBASLANGICTARIH)
        {
                ucret = ucret - PESINAT;
                int aylik_tutar = ucret / taksitsayisi;

                for (int i = 0; i < taksitsayisi; i++)
                {
                    TAKSIT t = new TAKSIT();
                    t.EGITIM_GRUP_REFNO = grupREFNO;
                    t.KAYIT_REFNO = kayitref;
                    t.BORC = aylik_tutar;
                    t.ODENEN = 0;
                    t.TARIH = PESINATBASLANGICTARIH.AddMonths(i);

                    db.TAKSITs.Add(t);
                }
                db.SaveChanges();
        }

        //Eğitim grubuna kaydolan kesin kayıt için doluluk bilgisi olusturma 
        public void EgitimeKisiEkle(int grupRefno)
        {
            EGITIM_GRUP e = db.EGITIM_GRUP.Find(grupRefno);
            e.EGITIM_GRUP_REFNO = grupRefno;
            e.DOLULUK = e.DOLULUK + 1;
            db.SaveChanges();
        }

        //Eğitim grubundan ayrılan kayıt için ayrılan bilgisi olusturma 
        public void EgitimdenKisiDus(int grupRefno)
        {
            EGITIM_GRUP e = db.EGITIM_GRUP.Find(grupRefno);
            e.EGITIM_GRUP_REFNO = grupRefno;
            e.AYRILAN = e.AYRILAN - 1;
            db.SaveChanges();
        }

        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Index", "Kayitlar", new { arama = txtAra });
        }
        public void ExportToExcel()       //excele çıkartma
        {

            List<KAYIT> emplist = db.KAYITs.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Kesin Kayıtlar Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "KayıtRefno";
            ws.Cells["B6"].Value = "EğitimGrupAdı";
            ws.Cells["C6"].Value = "KursiyerAdıSoyadı";
            ws.Cells["D6"].Value = "Açıklama";
            ws.Cells["E6"].Value = "KayıtDurumu";
            ws.Cells["F6"].Value = "KayıtÜcreti";
            ws.Cells["G6"].Value = "TaksitSayısı";


            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.KAYIT_REFNO <= db.KAYITs.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));
                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.KAYIT_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.EGITIM_GRUP.EGITIM_GRUP_ADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.KURSIYER.ADI_SOYADI;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ACIKLAMA;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.KAYIT_DURUMU;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.KAYIT_UCRETI;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.TAKSIT_SAYISI;


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
            var uListe = db.KAYITs.ToList();
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
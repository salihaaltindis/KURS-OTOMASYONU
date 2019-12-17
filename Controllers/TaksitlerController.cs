using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using KURSOTOMASYON.Models;
using OfficeOpenXml;
using Rotativa;

namespace KURSOTOMASYON.Controllers
{
    [Authorize]
    public class TaksitlerController : Controller
    {
        Model1 db = new Model1();
        //sayfadaki gözükecek satır sayısı
        int sayfadakisatirsayisi = 5;
        
        // GET: Taksitler
        public ActionResult Index(int id)
        {
            ViewData["id"] = id;
        
            //kayıtlı kursiyerin kalan borç tutarını görmek için 
            int kalanborc = db.TAKSITs.Where(t => t.KAYIT_REFNO == id).Sum(t1 => t1.BORC - t1.ODENEN);

            ViewData["kalanborc"] = kalanborc;

            List<VW_TAKSIT_KAYIT_KURSIYER> liste = new List<VW_TAKSIT_KAYIT_KURSIYER>();
            
            if (liste != null)
            {
                liste = db.VW_TAKSIT_KAYIT_KURSIYER.Where(k => k.KAYIT_REFNO == id).ToList();

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

        //kayıtlı kursiyerin taksitini ödemesi
        public ActionResult OdemeYap(int kayıtrefno, int tutar)
        {
            KAYIT kayıtliste = db.KAYITs.Find(kayıtrefno);

            int kalanborc = db.TAKSITs.Where(t => t.KAYIT_REFNO == kayıtrefno).Sum(t1 => t1.BORC - t1.ODENEN);

            List<TAKSIT> liste = db.TAKSITs.Where(a => a.KAYIT_REFNO == kayıtrefno && a.BORC>a.ODENEN).ToList();
            
            
            //FAZLA TAKSİT TUTARI YATIRMAYA CALISIRSA KİŞİ UYARI VERMESİNİ SAĞLAR.
            if(tutar > kalanborc)
            {
                    MessageBox.Show("HATA FAZLA PARA YATTI","ÖDEME YAPMA EKRANI",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {

            //KURSİYER HAREKET OLUSTURUP GUNCELLER
            HareketOlustur(kayıtrefno, tutar);

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
            }
            db.SaveChanges();
            
            return RedirectToAction("Index",new { id=kayıtrefno});
        }

        //kursiyer hareket i olusturup güncelleme
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

        //Kayıtlı tüm kursiyerlerin taksitlerini toplu görmek için olusturuldu
        public ActionResult Listele(string arama, int aktifsayfa = 0)
        {
            //Giris kontrolü için
            int yonetici_refno = Convert.ToInt32(Session["YONETICI_REFNO"]);
            int ygr = db.YONETICIs.Where(y1 => y1.YONETICI_REFNO == yonetici_refno).FirstOrDefault().YETKI_GRUBU_REFNO;
            int sayfa_refno = db.SAYFAs.Where(s => s.SAYFA_ADI == "Taksitler").SingleOrDefault().SAYFA_REFNO;
            bool yetki = YETKI.YetkiVarmi(ygr, sayfa_refno, YETKI.YETKI_TIPI.OKUMA);
            if (yetki == false) return RedirectToAction("Index", "Home");


            List<VW_TAKSIT_KAYIT_KURSIYER> liste = new List<VW_TAKSIT_KAYIT_KURSIYER>();
            if (arama == null)
            {
                arama = "";
                Sayfalama(db.VW_TAKSIT_KAYIT_KURSIYER.Count());
                liste = db.VW_TAKSIT_KAYIT_KURSIYER.OrderBy(u => u.TAKSIT_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            else
            {
                Sayfalama(db.VW_TAKSIT_KAYIT_KURSIYER.Where(k => k.ADI_SOYADI.Contains(arama)).Count());
                liste = db.VW_TAKSIT_KAYIT_KURSIYER.Where(k => k.ADI_SOYADI.Contains(arama)).OrderBy(u => u.TAKSIT_REFNO).Skip(aktifsayfa * sayfadakisatirsayisi).Take(sayfadakisatirsayisi).ToList();
            }
            ViewData["veri"] = arama;
            ViewData["aktifsayfa"] = aktifsayfa;
            return View(liste);
        }

        public ActionResult Delete(int id)
        {
            TAKSIT k = db.TAKSITs.Find(id);
            try
            {
                if (k != null)
                {
                    db.TAKSITs.Remove(k);
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
            TAKSIT urun = new TAKSIT();
            if (id != null)//guncelleme yapılacak
            {
                urun = db.TAKSITs.Find(id);
                if (urun == null)
                {
                    urun = new TAKSIT();
                }
            }
            var liste = db.KAYITs.ToList();
            ViewData["kayıt"] = liste;//hersey object olarak saklanır.
            ViewBag.kursiyer = db.KURSIYERs.ToList();
            return View(urun);
        }

        [HttpPost]//Kayıt
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(TAKSIT k)
        {
            if (ModelState.IsValid)
            {
                if (k.TAKSIT_REFNO == 0)
                {
                    db.TAKSITs.Add(k);
                }
                else
                {
                    db.Entry(k).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var liste = db.KAYITs.ToList();
            ViewData["kayıt"] = liste;//hersey object olarak saklanır.
            ViewBag.kursiyer = db.KURSIYERs.ToList();
            return View(k);
        }

        public ActionResult Search(string txtAra)
        {
            return RedirectToAction("Listele", "Taksitler", new { arama = txtAra });
        }

        public void ExportToExcel()
        {

            List<VW_TAKSIT_KAYIT_KURSIYER> emplist = db.VW_TAKSIT_KAYIT_KURSIYER.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Rapor";
            ws.Cells["B2"].Value = "Taksitler Excel Raporu";

            ws.Cells["A3"].Value = "Raporlama Tarihi";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "TaksitRefno";
            ws.Cells["B6"].Value = "AdıSoyadı";
            ws.Cells["C6"].Value = "Borç";
            ws.Cells["D6"].Value = "Ödenen";
            ws.Cells["E6"].Value = "Tarih";
            ws.Cells["F6"].Value = "Açıklama";
            ws.Cells["G6"].Value = "EğitimGrupAdı";



            int rowStart = 7;
            foreach (var item in emplist)
            {
                if (item.TAKSIT_REFNO <= db.VW_TAKSIT_KAYIT_KURSIYER.ToList().Count())
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("yellow")));

                }

                ws.Cells[string.Format("A{0}", rowStart)].Value = item.TAKSIT_REFNO;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.ADI_SOYADI;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.BORC;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.ODENEN;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.TARIH;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.ACIKLAMA;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.EGITIM_GRUP_ADI;

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
            var uListe = db.TAKSITs.ToList();
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

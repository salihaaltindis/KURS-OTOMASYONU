using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KURSOTOMASYON.Models
{
    public partial class YETKI
    {
        public enum YETKI_TIPI
        {
            OKUMA,KAYDET,SIL,ARAMA,YENI
        }
        public static bool YetkiVarmi(int yetki_grubu_refno,int sayfa_refno,YETKI_TIPI yetki_tipi)
        {
            Model1 db = new Model1();
            YETKI yetki = db.YETKIs.Where(y => y.SAYFA_REFNO == sayfa_refno && y.YETKI_GRUBU_REFNO == yetki_grubu_refno).SingleOrDefault();
            if (yetki == null)
            {
                return false;
            }
            switch (yetki_tipi)
            {
                case YETKI_TIPI.OKUMA:
                    return (yetki.OKUMA == true) ? true : false;
                case YETKI_TIPI.KAYDET:
                    return (yetki.KAYDET == true) ? true : false;
                case YETKI_TIPI.SIL:
                    return (yetki.SIL == true) ? true : false;
                case YETKI_TIPI.ARAMA:
                    return (yetki.ARAMA == true) ? true : false;
                case YETKI_TIPI.YENI:
                    return (yetki.YENI == true) ? true : false;
                default:
                    return false;
            }
        }
    }
}
namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("YETKI")]
    public partial class YETKI
    {
        [Key]
        public int YETKI_REFNO { get; set; }

        public int YETKI_GRUBU_REFNO { get; set; }

        public int SAYFA_REFNO { get; set; }

        public bool OKUMA { get; set; }

        public bool KAYDET { get; set; }

        public bool SIL { get; set; }

        public bool YENI { get; set; }

        public bool ARAMA { get; set; }

        public virtual SAYFA SAYFA { get; set; }

        public virtual YETKI_GRUBU YETKI_GRUBU { get; set; }
    }
}

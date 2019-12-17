namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EGITMEN_HAREKET
    {
        [Key]
        public int EGITMEN_HAREKET_REFNO { get; set; }

        public int EGITMEN_REFNO { get; set; }

        //[Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime TARIH { get; set; }

        public int ALACAK { get; set; }

        public int ODENEN { get; set; }

        [DisplayName("Açýklama")]
        [StringLength(200)]
        public string ACIKLAMA { get; set; }

        public virtual EGITMan EGITMan { get; set; }

        public virtual EGITMan EGITMan1 { get; set; }
    }
}

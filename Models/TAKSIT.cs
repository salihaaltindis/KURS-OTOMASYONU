namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TAKSIT")]
    public partial class TAKSIT
    {
        [Key]
        public int TAKSIT_REFNO { get; set; }

        public int KAYIT_REFNO { get; set; }

        //[Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime TARIH { get; set; }


        public int BORC { get; set; }

        public int ODENEN { get; set; }


        [StringLength(200)]
        public string ACIKLAMA { get; set; }

        public int EGITIM_GRUP_REFNO { get; set; }

        public virtual EGITIM_GRUP EGITIM_GRUP { get; set; }

        public virtual KAYIT KAYIT { get; set; }
    }
}

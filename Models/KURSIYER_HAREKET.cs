namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KURSIYER_HAREKET
    {
        [Key]
        public int KURSIYER_HAREKET_REFNO { get; set; }

        public int KAYIT_REFNO { get; set; }

        //[Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime TARIH { get; set; }


        public int BORC { get; set; }

        public int ODENEN { get; set; }

        public virtual KAYIT KAYIT { get; set; }

        public virtual KAYIT KAYIT1 { get; set; }
    }
}

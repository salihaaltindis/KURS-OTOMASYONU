namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("YOKLAMA")]
    public partial class YOKLAMA
    {
        [Key]
        public int YOKLAMA_REFNO { get; set; }

        public int KURSIYER_REFNO { get; set; }

        //[Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime TARIH { get; set; }

        public bool DURUMU { get; set; }

        [DisplayName("Açýklama"),MaxLength(50)]
        [StringLength(50)]
        public string ACIKLAMA { get; set; }

        public int KAYIT_REFNO { get; set; }

        public int? EGITIM_GRUP_REFNO { get; set; }

        public virtual KURSIYER KURSIYER { get; set; }
    }
}

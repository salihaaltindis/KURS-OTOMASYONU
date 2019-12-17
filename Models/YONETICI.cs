namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("YONETICI")]
    public partial class YONETICI
    {
        [Key]
        public int YONETICI_REFNO { get; set; }

        [Required]
        [StringLength(20)]
        public string YONETICI_ADI { get; set; }

        [Required]
        [StringLength(20)]
        public string PAROLA { get; set; }

        public bool DURUMU { get; set; }

        public int YETKI_GRUBU_REFNO { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        public virtual YETKI_GRUBU YETKI_GRUBU { get; set; }
    }
}

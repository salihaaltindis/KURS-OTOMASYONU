namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_YETKI_YETKI_GRUBU
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int YETKI_REFNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int YETKI_GRUBU_REFNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SAYFA_REFNO { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool OKUMA { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool KAYDET { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool SIL { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool ARAMA { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool YENI { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(20)]
        public string GRUP_ADI { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(30)]
        public string SAYFA_ADI { get; set; }
    }
}

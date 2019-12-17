namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_EGITMEN_HAREKET_EGITMEN
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITMEN_HAREKET_REFNO { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime TARIH { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITMEN_REFNO { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ALACAK { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ODENEN { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(30)]
        public string ADI_SOYADI { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool DURUM { get; set; }

        [StringLength(200)]
        public string ACIKLAMA { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(11)]
        public string TC { get; set; }
    }
}

namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string KATILMA_DURUMU { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KATILMA_DURUMU_REFNO { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime GORUSME_TARIHI { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(200)]
        public string ACIKLAMA { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string EMAIL { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(150)]
        public string ADRES { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(10)]
        public string TELEFON { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string ADI_SOYADI { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITIM_GRUP_REFNO { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ON_KAYIT_REFNO { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(150)]
        public string EGITIM_GRUP_ADI { get; set; }
    }
}

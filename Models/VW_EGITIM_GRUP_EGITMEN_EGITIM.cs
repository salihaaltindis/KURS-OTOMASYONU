namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_EGITIM_GRUP_EGITMEN_EGITIM
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string ADI_SOYADI { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITMEN_REFNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITIM_GRUP_REFNO { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UCRET { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime BASLANGIC_TARIH { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "date")]
        public DateTime BITIS_TARIH { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(5)]
        public string BASLANGIC_SAAT { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(5)]
        public string BITIS_SAAT { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KONTENJAN { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(150)]
        public string GUNLER { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITIM_REFNO { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(50)]
        public string EGITIM_ADI { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(150)]
        public string EGITIM_GRUP_ADI { get; set; }

        [Key]
        [Column(Order = 13)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DOLULUK { get; set; }

        [Key]
        [Column(Order = 14)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AYRILAN { get; set; }
    }
}

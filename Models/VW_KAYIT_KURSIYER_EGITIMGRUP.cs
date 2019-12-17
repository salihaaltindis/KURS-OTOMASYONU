namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_KAYIT_KURSIYER_EGITIMGRUP
    {
        [Key]
        [Column(Order = 0)]
        public bool KAYIT_DURUMU { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(500)]
        public string ACIKLAMA { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KURSIYER_REFNO { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITIM_GRUP_REFNO { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KAYIT_REFNO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(30)]
        public string Expr2 { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string EGITIM_ADI { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string ADI_SOYADI { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KAYIT_UCRETI { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITIM_REFNO { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITMEN_REFNO { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TAKSIT_SAYISI { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(150)]
        public string EGITIM_GRUP_ADI { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(11)]
        public string TC { get; set; }
    }
}

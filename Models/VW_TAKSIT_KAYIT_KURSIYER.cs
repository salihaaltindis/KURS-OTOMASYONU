namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_TAKSIT_KAYIT_KURSIYER
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KAYIT_REFNO { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime TARIH { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BORC { get; set; }

        [StringLength(200)]
        public string ACIKLAMA { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TAKSIT_REFNO { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string ADI_SOYADI { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ODENEN { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KURSIYER_REFNO { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool KAYIT_DURUMU { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITIM_GRUP_REFNO { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(150)]
        public string EGITIM_GRUP_ADI { get; set; }
    }
}

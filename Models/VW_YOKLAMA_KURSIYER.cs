namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_YOKLAMA_KURSIYER
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string ADI_SOYADI { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int YOKLAMA_REFNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KURSIYER_REFNO { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime TARIH { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool DURUMU { get; set; }

        [StringLength(50)]
        public string ACIKLAMA { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(11)]
        public string TC { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(150)]
        public string EGITIM_GRUP_ADI { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITIM_GRUP_REFNO { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KAYIT_REFNO { get; set; }
    }
}

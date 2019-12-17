namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_EGITIM_KATEGORI
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string KATEGORI_ADI { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EGITIM_REFNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KATEGORI_REFNO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(500)]
        public string ICERIK { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UCRET { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SAAT { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool DURUMU { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string EGITIM_ADI { get; set; }
    }
}

namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_KURSIYER_HAREKET_KAYIT_KURSIYER
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KURSIYER_REFNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ODENEN { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BORC { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime TARIH { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KAYIT_REFNO { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KURSIYER_HAREKET_REFNO { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool KAYIT_DURUMU { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string ADI_SOYADI { get; set; }
    }
}

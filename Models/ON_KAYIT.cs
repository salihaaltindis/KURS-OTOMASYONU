namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ON_KAYIT
    {
        [Key]
        public int ON_KAYIT_REFNO { get; set; }

        public int EGITIM_GRUP_REFNO { get; set; }

        [Required(ErrorMessage ="{0} giriniz."),MaxLength(50)]
        [DisplayName("Adý soyadý")]
        [StringLength(50)]
        public string ADI_SOYADI { get; set; }

        [Required(ErrorMessage ="{0} giriniz."),MaxLength(10)]
        [DisplayName("Telefon")]
        [StringLength(10)]
        public string TELEFON { get; set; }

        [Required(ErrorMessage ="{0} giriniz."),MaxLength(150)]
        [DisplayName("Adres")]
        [StringLength(150)]
        public string ADRES { get; set; }

        [Required(ErrorMessage ="{0} giriniz."),MaxLength(50)]
        [DisplayName("Email")]
        [StringLength(50)]
        public string EMAIL { get; set; }

        [Required(ErrorMessage ="{0} giriniz."),MaxLength(200)]
        [DisplayName("Açýklama")]
        [StringLength(200)]
        public string ACIKLAMA { get; set; }

        //[Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime GORUSME_TARIHI { get; set; }

        public int KATILMA_DURUMU_REFNO { get; set; }

        public virtual EGITIM_GRUP EGITIM_GRUP { get; set; }

        public virtual KATILMA_DURUMU KATILMA_DURUMU { get; set; }
    }
}

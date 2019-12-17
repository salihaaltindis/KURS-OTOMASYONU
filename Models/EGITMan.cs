namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EGITMEN")]
    public partial class EGITMan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EGITMan()
        {
            EGITIM_GRUP = new HashSet<EGITIM_GRUP>();
            EGITMEN_HAREKET = new HashSet<EGITMEN_HAREKET>();
            EGITMEN_HAREKET1 = new HashSet<EGITMEN_HAREKET>();
        }

        [Key]
        public int EGITMEN_REFNO { get; set; }

        [Required(ErrorMessage = "{0} giriniz.")]
        [DisplayName("Adý Soyadý"), MaxLength(30)]
        [StringLength(30)]
        public string ADI_SOYADI { get; set; }

        [Required(ErrorMessage = "{0} giriniz."), MaxLength(11)]
        [StringLength(11)]
        public string TC { get; set; }

        [Required(ErrorMessage = "{0} giriniz."), MaxLength(20)]
        [DisplayName("Parola")]
        [StringLength(20)]
        public string PAROLA { get; set; }

        [Required(ErrorMessage = "{0} ekleyiniz."), MaxLength(10)]
        [DisplayName("Telefon")]
        [StringLength(10)]
        public string TELEFON { get; set; }

        [Required(ErrorMessage = "{0} ekleyiniz."), MaxLength(50), DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        [StringLength(50)]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "{0} ekleyiniz."), MaxLength(150)]
        [DisplayName("Adres")]
        [StringLength(150)]
        public string ADRES { get; set; }

        public bool DURUM { get; set; }

        [Range(1, 99999)]
        public int UCRET { get; set; }

        [Required(ErrorMessage = "{0} giriniz.")]
        [DisplayName("Açýklama")]
        public string ACIKLAMA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EGITIM_GRUP> EGITIM_GRUP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EGITMEN_HAREKET> EGITMEN_HAREKET { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EGITMEN_HAREKET> EGITMEN_HAREKET1 { get; set; }
    }
}

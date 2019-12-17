namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EGITIM_GRUP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EGITIM_GRUP()
        {
            KAYITs = new HashSet<KAYIT>();
            KAYITs1 = new HashSet<KAYIT>();
            ON_KAYIT = new HashSet<ON_KAYIT>();
            TAKSITs = new HashSet<TAKSIT>();
        }

        [Key]
        public int EGITIM_GRUP_REFNO { get; set; }

        public int EGITIM_REFNO { get; set; }

        public int EGITMEN_REFNO { get; set; }

        [DisplayName("Ücret")]
        [Range(1, 99999)]
        public int UCRET { get; set; }

        //[Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime BASLANGIC_TARIH { get; set; }

        //[Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime BITIS_TARIH { get; set; }

        [Required(ErrorMessage ="{0} giriniz."),MaxLength(5)]
        [DisplayName("Baþlangýç Saati")]
        [StringLength(5)]
        public string BASLANGIC_SAAT { get; set; }

        [Required(ErrorMessage = "{0} giriniz."), MaxLength(5)]
        [DisplayName("Bitiþ Saati")]
        [StringLength(5)]
        public string BITIS_SAAT { get; set; }

        [Range(1,99999)]
        public int KONTENJAN { get; set; }

        [Required(ErrorMessage ="{0} ekleyiniz."),MaxLength(150)]
        [DisplayName("Günler")]
        [StringLength(150)]
        public string GUNLER { get; set; }

        [Required(ErrorMessage ="{0} giriniz.")]
        [DisplayName("Eðitim grup adý")]
        [StringLength(150)]
        public string EGITIM_GRUP_ADI { get; set; }

        [DisplayName("Doluluk")]
        public int DOLULUK { get; set; }

        [DisplayName("Ayrýlan")]
        public int AYRILAN { get; set; }

        public virtual EGITIM EGITIM { get; set; }

        public virtual EGITMan EGITMan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KAYIT> KAYITs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KAYIT> KAYITs1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ON_KAYIT> ON_KAYIT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAKSIT> TAKSITs { get; set; }
    }
}

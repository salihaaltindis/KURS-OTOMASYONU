namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KAYIT")]
    public partial class KAYIT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KAYIT()
        {
            KURSIYER_HAREKET = new HashSet<KURSIYER_HAREKET>();
            KURSIYER_HAREKET1 = new HashSet<KURSIYER_HAREKET>();
            TAKSITs = new HashSet<TAKSIT>();
        }

        [Key]
        public int KAYIT_REFNO { get; set; }

        public int EGITIM_GRUP_REFNO { get; set; }

        public int KURSIYER_REFNO { get; set; }

        [Required(ErrorMessage ="{0} ekleyiniz."),MaxLength(500)]
        [DisplayName("Açýklama")]
        [StringLength(500)]
        public string ACIKLAMA { get; set; }

        public bool KAYIT_DURUMU { get; set; }

        [DisplayName("Kayýt Ücreti")]
        [Range(1,99999)]
        public int KAYIT_UCRETI { get; set; }

        [DisplayName("Taksit Sayýsý")]
        [Range(1, 99999)]
        public int TAKSIT_SAYISI { get; set; }

        public virtual EGITIM_GRUP EGITIM_GRUP { get; set; }

        public virtual EGITIM_GRUP EGITIM_GRUP1 { get; set; }

        public virtual KURSIYER KURSIYER { get; set; }

        public virtual KURSIYER KURSIYER1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KURSIYER_HAREKET> KURSIYER_HAREKET { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KURSIYER_HAREKET> KURSIYER_HAREKET1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAKSIT> TAKSITs { get; set; }
    }
}

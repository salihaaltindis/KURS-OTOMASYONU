namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EGITIM")]
    public partial class EGITIM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EGITIM()
        {
            EGITIM_GRUP = new HashSet<EGITIM_GRUP>();
        }

        [Key]
        public int EGITIM_REFNO { get; set; }

        [Required(ErrorMessage ="{0} giriniz.")]
        [DisplayName("Eðitim Adý"),MaxLength(50)]
        [StringLength(50)]
        public string EGITIM_ADI { get; set; }

        public int KATEGORI_REFNO { get; set; }

        [Required(ErrorMessage ="{0} alaný ekleyiniz.")]
        [DisplayName("Ýçerik"),MaxLength(500)]
        [StringLength(500)]
        public string ICERIK { get; set; }
        
        [Range(1,99999)]
        public int UCRET { get; set; }

        [DisplayName("Saat")]
        [Range(1,99999)]
        public int SAAT { get; set; }

        public bool DURUMU { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EGITIM_GRUP> EGITIM_GRUP { get; set; }

        public virtual KATEGORI KATEGORI { get; set; }
    }
}

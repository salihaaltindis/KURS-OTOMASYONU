namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KATEGORI")]
    public partial class KATEGORI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KATEGORI()
        {
            EGITIMs = new HashSet<EGITIM>();
        }

        [Key]
        public int KATEGORI_REFNO { get; set; }

        [Required(ErrorMessage = "{0} giriniz.")]
        [DisplayName("Kategori Adý"), MaxLength(50)]
        [StringLength(50)]
        public string KATEGORI_ADI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EGITIM> EGITIMs { get; set; }
    }
}

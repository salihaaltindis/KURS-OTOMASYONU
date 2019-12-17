namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class YETKI_GRUBU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public YETKI_GRUBU()
        {
            YETKIs = new HashSet<YETKI>();
            YONETICIs = new HashSet<YONETICI>();
        }

        [Key]
        public int YETKI_GRUBU_REFNO { get; set; }

        [Required]
        [StringLength(20)]
        public string GRUP_ADI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YETKI> YETKIs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YONETICI> YONETICIs { get; set; }
    }
}

namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SAYFA")]
    public partial class SAYFA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SAYFA()
        {
            YETKIs = new HashSet<YETKI>();
        }

        [Key]
        public int SAYFA_REFNO { get; set; }

        [Required]
        [StringLength(30)]
        public string SAYFA_ADI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YETKI> YETKIs { get; set; }
    }
}

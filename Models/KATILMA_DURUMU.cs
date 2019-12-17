namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class KATILMA_DURUMU
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KATILMA_DURUMU()
        {
            ON_KAYIT = new HashSet<ON_KAYIT>();
        }

        [Key]
        public int KATILMA_DURUMU_REFNO { get; set; }

        [Column("KATILMA_DURUMU")]
        [Required(ErrorMessage = "{0} giriniz.")]
        [DisplayName("Katýlma Durumu") , MaxLength(50)]
        [StringLength(50)]
        public string KATILMA_DURUMU1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ON_KAYIT> ON_KAYIT { get; set; }
    }
}

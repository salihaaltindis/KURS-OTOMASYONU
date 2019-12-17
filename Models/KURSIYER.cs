namespace KURSOTOMASYON.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KURSIYER")]
    public partial class KURSIYER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KURSIYER()
        {
            KAYITs = new HashSet<KAYIT>();
            KAYITs1 = new HashSet<KAYIT>();
            YOKLAMAs = new HashSet<YOKLAMA>();
        }

        [Key]
        public int KURSIYER_REFNO { get; set; }

        [Required(ErrorMessage ="{0} giriniz.")]
        [DisplayName("Adý Soyadý"),MaxLength(50)]
        [StringLength(50)]
        public string ADI_SOYADI { get; set; }

        [Required(ErrorMessage ="{0} giriniz."),MaxLength(11)]
        [StringLength(11)]
        public string TC { get; set; }

        [Required(ErrorMessage ="{0} ekleyiniz."),MaxLength(150)]
        [DisplayName("Adres")]
        [StringLength(150)]
        public string ADRES { get; set; }

        [Required(ErrorMessage ="{0} alaný giriniz."),MaxLength(10)]
        [DisplayName("Telefon")]
        [StringLength(10)]
        public string TELEFON { get; set; }

        [Required(ErrorMessage ="{0} alaný giriniz."),MaxLength(20)]
        [DisplayName("Parola")]
        [StringLength(20)]
        public string PAROLA { get; set; }

        [Required(ErrorMessage ="{0} alaný giriniz."),MaxLength(50),DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        [StringLength(50)]
        public string EMAIL { get; set; }

        public bool CINSIYET { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KAYIT> KAYITs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KAYIT> KAYITs1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<YOKLAMA> YOKLAMAs { get; set; }
    }
}

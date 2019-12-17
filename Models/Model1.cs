namespace KURSOTOMASYON.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<EGITIM> EGITIMs { get; set; }
        public virtual DbSet<EGITIM_GRUP> EGITIM_GRUP { get; set; }
        public virtual DbSet<EGITMan> EGITMEN { get; set; }
        public virtual DbSet<EGITMEN_HAREKET> EGITMEN_HAREKET { get; set; }
        public virtual DbSet<KATEGORI> KATEGORIs { get; set; }
        public virtual DbSet<KATILMA_DURUMU> KATILMA_DURUMU { get; set; }
        public virtual DbSet<KAYIT> KAYITs { get; set; }
        public virtual DbSet<KURSIYER> KURSIYERs { get; set; }
        public virtual DbSet<KURSIYER_HAREKET> KURSIYER_HAREKET { get; set; }
        public virtual DbSet<ON_KAYIT> ON_KAYIT { get; set; }
        public virtual DbSet<SAYFA> SAYFAs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TAKSIT> TAKSITs { get; set; }
        public virtual DbSet<YETKI> YETKIs { get; set; }
        public virtual DbSet<YETKI_GRUBU> YETKI_GRUBU { get; set; }
        public virtual DbSet<YOKLAMA> YOKLAMAs { get; set; }
        public virtual DbSet<YONETICI> YONETICIs { get; set; }
        public virtual DbSet<VW_EGITIM_GRUP_EGITMEN_EGITIM> VW_EGITIM_GRUP_EGITMEN_EGITIM { get; set; }
        public virtual DbSet<VW_EGITIM_KATEGORI> VW_EGITIM_KATEGORI { get; set; }
        public virtual DbSet<VW_EGITMEN_HAREKET_EGITMEN> VW_EGITMEN_HAREKET_EGITMEN { get; set; }
        public virtual DbSet<VW_KAYIT_KURSIYER_EGITIMGRUP> VW_KAYIT_KURSIYER_EGITIMGRUP { get; set; }
        public virtual DbSet<VW_KURSIYER_HAREKET_KAYIT_KURSIYER> VW_KURSIYER_HAREKET_KAYIT_KURSIYER { get; set; }
        public virtual DbSet<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU> VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU { get; set; }
        public virtual DbSet<VW_TAKSIT_KAYIT_KURSIYER> VW_TAKSIT_KAYIT_KURSIYER { get; set; }
        public virtual DbSet<VW_YETKI_YETKI_GRUBU> VW_YETKI_YETKI_GRUBU { get; set; }
        public virtual DbSet<VW_YOKLAMA_KURSIYER> VW_YOKLAMA_KURSIYER { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EGITIM>()
                .Property(e => e.EGITIM_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<EGITIM>()
                .Property(e => e.ICERIK)
                .IsUnicode(false);

            modelBuilder.Entity<EGITIM>()
                .HasMany(e => e.EGITIM_GRUP)
                .WithRequired(e => e.EGITIM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EGITIM_GRUP>()
                .Property(e => e.BASLANGIC_SAAT)
                .IsUnicode(false);

            modelBuilder.Entity<EGITIM_GRUP>()
                .Property(e => e.BITIS_SAAT)
                .IsUnicode(false);

            modelBuilder.Entity<EGITIM_GRUP>()
                .Property(e => e.GUNLER)
                .IsUnicode(false);

            modelBuilder.Entity<EGITIM_GRUP>()
                .Property(e => e.EGITIM_GRUP_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<EGITIM_GRUP>()
                .HasMany(e => e.KAYITs)
                .WithRequired(e => e.EGITIM_GRUP)
                .HasForeignKey(e => e.EGITIM_GRUP_REFNO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EGITIM_GRUP>()
                .HasMany(e => e.KAYITs1)
                .WithRequired(e => e.EGITIM_GRUP1)
                .HasForeignKey(e => e.EGITIM_GRUP_REFNO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EGITIM_GRUP>()
                .HasMany(e => e.ON_KAYIT)
                .WithRequired(e => e.EGITIM_GRUP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EGITIM_GRUP>()
                .HasMany(e => e.TAKSITs)
                .WithRequired(e => e.EGITIM_GRUP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EGITMan>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<EGITMan>()
                .Property(e => e.TC)
                .IsUnicode(false);

            modelBuilder.Entity<EGITMan>()
                .Property(e => e.PAROLA)
                .IsUnicode(false);

            modelBuilder.Entity<EGITMan>()
                .Property(e => e.TELEFON)
                .IsUnicode(false);

            modelBuilder.Entity<EGITMan>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<EGITMan>()
                .Property(e => e.ADRES)
                .IsUnicode(false);

            modelBuilder.Entity<EGITMan>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<EGITMan>()
                .HasMany(e => e.EGITIM_GRUP)
                .WithRequired(e => e.EGITMan)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EGITMan>()
                .HasMany(e => e.EGITMEN_HAREKET)
                .WithRequired(e => e.EGITMan)
                .HasForeignKey(e => e.EGITMEN_REFNO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EGITMan>()
                .HasMany(e => e.EGITMEN_HAREKET1)
                .WithRequired(e => e.EGITMan1)
                .HasForeignKey(e => e.EGITMEN_REFNO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EGITMEN_HAREKET>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<KATEGORI>()
                .Property(e => e.KATEGORI_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<KATEGORI>()
                .HasMany(e => e.EGITIMs)
                .WithRequired(e => e.KATEGORI)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KATILMA_DURUMU>()
                .Property(e => e.KATILMA_DURUMU1)
                .IsUnicode(false);

            modelBuilder.Entity<KATILMA_DURUMU>()
                .HasMany(e => e.ON_KAYIT)
                .WithRequired(e => e.KATILMA_DURUMU)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KAYIT>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<KAYIT>()
                .HasMany(e => e.KURSIYER_HAREKET)
                .WithRequired(e => e.KAYIT)
                .HasForeignKey(e => e.KAYIT_REFNO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KAYIT>()
                .HasMany(e => e.KURSIYER_HAREKET1)
                .WithRequired(e => e.KAYIT1)
                .HasForeignKey(e => e.KAYIT_REFNO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KAYIT>()
                .HasMany(e => e.TAKSITs)
                .WithRequired(e => e.KAYIT)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KURSIYER>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<KURSIYER>()
                .Property(e => e.TC)
                .IsUnicode(false);

            modelBuilder.Entity<KURSIYER>()
                .Property(e => e.ADRES)
                .IsUnicode(false);

            modelBuilder.Entity<KURSIYER>()
                .Property(e => e.TELEFON)
                .IsUnicode(false);

            modelBuilder.Entity<KURSIYER>()
                .Property(e => e.PAROLA)
                .IsUnicode(false);

            modelBuilder.Entity<KURSIYER>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<KURSIYER>()
                .HasMany(e => e.KAYITs)
                .WithRequired(e => e.KURSIYER)
                .HasForeignKey(e => e.KURSIYER_REFNO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KURSIYER>()
                .HasMany(e => e.KAYITs1)
                .WithRequired(e => e.KURSIYER1)
                .HasForeignKey(e => e.KURSIYER_REFNO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KURSIYER>()
                .HasMany(e => e.YOKLAMAs)
                .WithRequired(e => e.KURSIYER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ON_KAYIT>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<ON_KAYIT>()
                .Property(e => e.TELEFON)
                .IsUnicode(false);

            modelBuilder.Entity<ON_KAYIT>()
                .Property(e => e.ADRES)
                .IsUnicode(false);

            modelBuilder.Entity<ON_KAYIT>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<ON_KAYIT>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<SAYFA>()
                .Property(e => e.SAYFA_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<SAYFA>()
                .HasMany(e => e.YETKIs)
                .WithRequired(e => e.SAYFA)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAKSIT>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<YETKI_GRUBU>()
                .Property(e => e.GRUP_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<YETKI_GRUBU>()
                .HasMany(e => e.YETKIs)
                .WithRequired(e => e.YETKI_GRUBU)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<YETKI_GRUBU>()
                .HasMany(e => e.YONETICIs)
                .WithRequired(e => e.YETKI_GRUBU)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<YOKLAMA>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<YONETICI>()
                .Property(e => e.YONETICI_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<YONETICI>()
                .Property(e => e.PAROLA)
                .IsUnicode(false);

            modelBuilder.Entity<YONETICI>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_GRUP_EGITMEN_EGITIM>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_GRUP_EGITMEN_EGITIM>()
                .Property(e => e.BASLANGIC_SAAT)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_GRUP_EGITMEN_EGITIM>()
                .Property(e => e.BITIS_SAAT)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_GRUP_EGITMEN_EGITIM>()
                .Property(e => e.GUNLER)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_GRUP_EGITMEN_EGITIM>()
                .Property(e => e.EGITIM_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_GRUP_EGITMEN_EGITIM>()
                .Property(e => e.EGITIM_GRUP_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_KATEGORI>()
                .Property(e => e.KATEGORI_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_KATEGORI>()
                .Property(e => e.ICERIK)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITIM_KATEGORI>()
                .Property(e => e.EGITIM_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITMEN_HAREKET_EGITMEN>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITMEN_HAREKET_EGITMEN>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<VW_EGITMEN_HAREKET_EGITMEN>()
                .Property(e => e.TC)
                .IsUnicode(false);

            modelBuilder.Entity<VW_KAYIT_KURSIYER_EGITIMGRUP>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<VW_KAYIT_KURSIYER_EGITIMGRUP>()
                .Property(e => e.Expr2)
                .IsUnicode(false);

            modelBuilder.Entity<VW_KAYIT_KURSIYER_EGITIMGRUP>()
                .Property(e => e.EGITIM_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_KAYIT_KURSIYER_EGITIMGRUP>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_KAYIT_KURSIYER_EGITIMGRUP>()
                .Property(e => e.EGITIM_GRUP_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_KAYIT_KURSIYER_EGITIMGRUP>()
                .Property(e => e.TC)
                .IsUnicode(false);

            modelBuilder.Entity<VW_KURSIYER_HAREKET_KAYIT_KURSIYER>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU>()
                .Property(e => e.KATILMA_DURUMU)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU>()
                .Property(e => e.ADRES)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU>()
                .Property(e => e.TELEFON)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ONKAYIT_EGITIMGRUP_KATILMADURUMU>()
                .Property(e => e.EGITIM_GRUP_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_TAKSIT_KAYIT_KURSIYER>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<VW_TAKSIT_KAYIT_KURSIYER>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_TAKSIT_KAYIT_KURSIYER>()
                .Property(e => e.EGITIM_GRUP_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_YETKI_YETKI_GRUBU>()
                .Property(e => e.GRUP_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_YETKI_YETKI_GRUBU>()
                .Property(e => e.SAYFA_ADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_YOKLAMA_KURSIYER>()
                .Property(e => e.ADI_SOYADI)
                .IsUnicode(false);

            modelBuilder.Entity<VW_YOKLAMA_KURSIYER>()
                .Property(e => e.ACIKLAMA)
                .IsUnicode(false);

            modelBuilder.Entity<VW_YOKLAMA_KURSIYER>()
                .Property(e => e.TC)
                .IsUnicode(false);

            modelBuilder.Entity<VW_YOKLAMA_KURSIYER>()
                .Property(e => e.EGITIM_GRUP_ADI)
                .IsUnicode(false);
        }
    }
}

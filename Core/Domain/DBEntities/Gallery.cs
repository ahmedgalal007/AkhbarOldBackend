// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Gallery
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  [Table("Gallery")]
  public class Gallery
  {
    public Gallery()
    {
      this.NewsPictureLst = (ICollection<NewsPictures>) new HashSet<NewsPictures>();
      this.NewsLst = (ICollection<News>) new HashSet<News>();
    }

    public int GalleryID { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال اسم الالبوم")]
    [Display(Name = "اسم الالبوم")]
    public string Title { get; set; }

    [StringLength(500)]
    [Required(ErrorMessage = "تأكد من ادخال وصف الالبوم")]
    [Display(Name = "وصف الالبوم")]
    public string Description { get; set; }

    [StringLength(200)]
    [Required(ErrorMessage = "الرجاء إضافة الكلمات الدالة")]
    [Display(Name = "الكلمات الدالة")]
    public string Keywords { get; set; }

    [Display(Name = "تاريخ الإنشاء")]
    public DateTime? GDate { get; set; }

    public int? MainPictureID { get; set; }

    [Display(Name = "ألبوم رئيسي")]
    public bool? IsHome { get; set; }

    [StringLength(50)]
    [Display(Name = "نوع الألبوم")]
    public string GalleryType { get; set; }

    public int? JournalID { get; set; }

    public int? DisplayOrder { get; set; }

    [ForeignKey("GalleryType")]
    public virtual Akhbar.DBEntities.GalleryType GalleryType1 { get; set; }

    public virtual ICollection<NewsPictures> NewsPictureLst { get; set; }

    public virtual ICollection<GalleryPictures> GalleryPictureLst { get; set; }

    public virtual ICollection<News> NewsLst { get; set; }
  }
}

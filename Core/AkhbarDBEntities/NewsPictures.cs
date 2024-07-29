// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.NewsPictures
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class NewsPictures
  {
    public NewsPictures()
    {
      this.GalleryLst = (ICollection<Gallery>) new HashSet<Gallery>();
      this.NewsLst = (ICollection<News>) new HashSet<News>();
      this.NewsVersionsLst = (ICollection<NewsVersions>) new HashSet<NewsVersions>();
    }

    [Key]
    public int PictureID { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال الصورة")]
    [Display(Name = " الصورة")]
    public string PictureName { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال الكلمات الدالة")]
    [Display(Name = "الكلمات الدالة")]
    public string KeyWords { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال عنوان الصورة")]
    [Display(Name = "عنوان الصورة")]
    public string PicCaption { get; set; }

    public int? CatID { get; set; }

    public DateTime? added_date { get; set; }

    public byte? Source { get; set; }

    public int? JournalID { get; set; }

    public virtual ICollection<Gallery> GalleryLst { get; set; }

    public virtual ICollection<News> NewsLst { get; set; }

    public virtual ICollection<NewsVersions> NewsVersionsLst { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Profile
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  [Table("Profile")]
  public class Profile
  {
    [Key]
    public int ProfileId { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال اسم الملف")]
    [Display(Name = "اسم الملف")]
    public string ProfName { get; set; }

    [StringLength(200)]
    [Display(Name = "اسم الصورة")]
    public string MainPictureName { get; set; }

    [Display(Name = "إخفاء")]
    public byte? Hide { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال الكلمات الدالة")]
    [Display(Name = "الكلمات الدالة")]
    public string Keyword { get; set; }

    [StringLength(200)]
    [Required(ErrorMessage = "تأكد من ادخال وصف الملف")]
    [Display(Name = "وصف الملف")]
    public string Description { get; set; }

    public int? DisplayOrder { get; set; }

    public DateTime? CreationDate { get; set; }
  }
}

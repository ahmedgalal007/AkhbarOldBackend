// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.BlocksZone
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class BlocksZone
  {
    [Key]
    public int ZoneID { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال الاسم")]
    [Display(Name = "اسم المساحة الاعلانية")]
    public string ZoneName { get; set; }

    [StringLength(1000)]
    [Required(ErrorMessage = "تأكد من ادخال التوصيف المناسب ")]
    [Display(Name = "التوصيف")]
    public string ZoneDescription { get; set; }

    [Display(Name = "تاريخ الاضافة")]
    public DateTime? AddedDate { get; set; }

    public int AddedBy { get; set; }
  }
}

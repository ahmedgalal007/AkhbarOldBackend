// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.NewsBlocks
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  public class NewsBlocks
  {
    [Key]
    public int BlockID { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال عنوان المساحة الاعلانية")]
    [Display(Name = "عنوان المساحة الاعلانية")]
    public string BlockName { get; set; }

    [Column(TypeName = "text")]
    public string BlockText { get; set; }

    public int? JournalID { get; set; }

    public int? ZoneID { get; set; }

    [Required(ErrorMessage = "تأكد من ادخال تاريخ بدء الحملة")]
    [Display(Name = "تاريخ بدء الحملة")]
    public DateTime? FromDate { get; set; }

    [Required(ErrorMessage = "تأكد من ادخال تاريخ نهاية الحملة")]
    [Display(Name = "تاريخ نهاية الحملة")]
    public DateTime? ToDate { get; set; }
  }
}

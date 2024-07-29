// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.NewsTicker
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  [Table("NewsTicker")]
  public class NewsTicker
  {
    [Key]
    public int NewID { get; set; }

    [StringLength(1000)]
    [Display(Name = "العنوان بشريط الاخبار")]
    public string Title { get; set; }

    public int? NewsID { get; set; }

    [Display(Name = "تاريخ الاضافة")]
    public DateTime? Added_Date { get; set; }

    public int? JournalID { get; set; }

    public int? SectionId { get; set; }
  }
}

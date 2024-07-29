// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Journal
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  [Table("Journal")]
  public class Journal
  {
    public int JournalID { get; set; }

    [StringLength(100)]
    [Display(Name = "اسم الاصدار")]
    public string Name { get; set; }

    [StringLength(250)]
    public string FBToken { get; set; }

    [StringLength(50)]
    [Display(Name = "مقاس الصورة الكبيرة")]
    public string ImageLargeSize { get; set; }

    [StringLength(50)]
    [Display(Name = "مقاس الصورة الصغيرة")]
    public string ImageSmallSize { get; set; }

    [StringLength(50)]
    public string ImageSliderSize { get; set; }

    [StringLength(50)]
    public string ComicLargeSize { get; set; }

    [StringLength(50)]
    public string ComicSmallSize { get; set; }

    [StringLength(50)]
    public string EditorLargeSize { get; set; }

    [StringLength(50)]
    public string EditorSmallSize { get; set; }

    [StringLength(50)]
    public string PainterLargeSize { get; set; }

    [StringLength(50)]
    public string PainterSmallSize { get; set; }
  }
}

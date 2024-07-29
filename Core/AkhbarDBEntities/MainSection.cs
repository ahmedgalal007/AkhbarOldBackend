// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.MainSection
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class MainSection
  {
    public MainSection() => this.NewsSectionLst = (ICollection<NewsSection>) new HashSet<NewsSection>();

    [Key]
    public int SectionID { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال اسم الباب")]
    [Display(Name = "اسم الباب")]
    public string SecTitle { get; set; }

    [Display(Name = "إخفاء")]
    public byte? Hide { get; set; }

    [Display(Name = "إسبوعي")]
    public byte? WeeklySection { get; set; }

    public int? DisplayOrder { get; set; }

    public int? JournalID { get; set; }

    [StringLength(200)]
    [Required(ErrorMessage = "تأكد من ادخال الكلمات الدالة")]
    [Display(Name = "الكلمات الدالة")]
    public string Keywords { get; set; }

    [StringLength(200)]
    [Display(Name = "وصف الباب")]
    public string Description { get; set; }

    public int? ParentSectionID { get; set; }

    public virtual ICollection<NewsSection> NewsSectionLst { get; set; }

    public virtual ICollection<Editor> EditorsLst { get; set; }
  }
}

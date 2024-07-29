// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.ByLine
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  [Table("ByLine")]
  public class ByLine
  {
    public ByLine() => this.NewsLst = (ICollection<News>) new HashSet<News>();

    public int ByLineId { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال اسم المحرر")]
    [Display(Name = "اسم المحرر")]
    public string ByLineName { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<News> NewsLst { get; set; }
  }
}

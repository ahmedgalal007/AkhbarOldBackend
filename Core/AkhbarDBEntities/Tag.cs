// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Tag
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class Tag
  {
    public Tag()
    {
      this.NewsLst = (ICollection<News>) new HashSet<News>();
      this.NewsVersionsLst = (ICollection<NewsVersions>) new HashSet<NewsVersions>();
    }

    public long ID { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال الكلمات الدالة")]
    [Display(Name = "الكلمة الدالة")]
    public string Name { get; set; }

    public virtual ICollection<News> NewsLst { get; set; }

    public virtual ICollection<NewsVersions> NewsVersionsLst { get; set; }
  }
}

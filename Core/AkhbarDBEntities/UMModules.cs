// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.UMModules
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class UMModules
  {
    [Key]
    public int ModuleId { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال اسم القطاع")]
    [Display(Name = "اسم القطاع")]
    public string ModuleName { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال اسم المنطقة")]
    [Display(Name = "اسم المنطقة")]
    public string AreaName { get; set; }

    public bool? ModuleHide { get; set; }

    public virtual ICollection<UMServices> UMServicesLst { get; set; }

    public UMModules() => this.UMServicesLst = (ICollection<UMServices>) new HashSet<UMServices>();
  }
}

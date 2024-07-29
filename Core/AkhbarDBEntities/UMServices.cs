// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.UMServices
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class UMServices
  {
    [Key]
    public int ServiceId { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال اسم الخدمة")]
    [Display(Name = "اسم الخدمة")]
    public string ServiceName { get; set; }

    [Display(Name = "القطاع")]
    public int? ModuleId { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال اسم الكونترول")]
    [Display(Name = "اسم الكونترول")]
    public string ControllerName { get; set; }

    [StringLength(50)]
    public string ActionName { get; set; }

    public bool? ServiceHide { get; set; }

    [StringLength(50)]
    public string RelatedService { get; set; }

    public virtual UMModules UMModule { get; set; }

    public virtual ICollection<Role> RoleLst { get; set; }

    public UMServices() => this.RoleLst = (ICollection<Role>) new HashSet<Role>();
  }
}

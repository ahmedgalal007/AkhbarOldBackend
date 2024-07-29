// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Role
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Akhbar.DBEntities
{
  public class Role
  {
    [Key]
    public int NewID { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال مسمي الدور الوظيفي")]
    [Display(Name = "الدور الوظيفي")]
    public string RoleName { get; set; }

    public int DisplayOrder { get; set; }

    public virtual ICollection<UserRole> UserRolesLst { get; set; }

    public virtual ICollection<UMServices> UMServicesLst { get; set; }

    public virtual ICollection<RoleServices> RoleServiceLst { get; set; }
  }
}

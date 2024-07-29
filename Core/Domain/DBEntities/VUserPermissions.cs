// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.VUserPermissions
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  public class VUserPermissions
  {
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int UserID { get; set; }

    [Key]
    [Column(Order = 1)]
    [StringLength(50)]
    public string UserName { get; set; }

    [Key]
    [Column(Order = 2)]
    [StringLength(50)]
    public string Password { get; set; }

    [Key]
    [Column(Order = 6)]
    [StringLength(400)]
    public string FullName { get; set; }

    [Key]
    [Column(Order = 3)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int RoleID { get; set; }

    [StringLength(50)]
    public string RoleName { get; set; }

    [Key]
    [Column(Order = 4)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ServiceId { get; set; }

    [StringLength(50)]
    public string ServiceName { get; set; }

    [Key]
    [Column(Order = 5)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ModuleId { get; set; }

    [StringLength(50)]
    public string ModuleName { get; set; }

    [StringLength(50)]
    public string AreaName { get; set; }

    [StringLength(50)]
    public string ControllerName { get; set; }

    [StringLength(50)]
    public string ActionName { get; set; }

    [StringLength(20)]
    public string ModuleIcon { get; set; }

    [StringLength(20)]
    public string ServiceIcon { get; set; }

    public bool? ServiceHide { get; set; }

    public bool? ModuleHide { get; set; }
  }
}

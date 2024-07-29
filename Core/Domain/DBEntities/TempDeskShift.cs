// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.TempDeskShift
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  [Table("TempDeskShift")]
  public class TempDeskShift
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int UserId { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderID { get; set; }

    public bool AssignFlag { get; set; }

    public bool WaitingFlag { get; set; }

    [ForeignKey("UserId")]
    public virtual AdminUser AdminUser { get; set; }
  }
}

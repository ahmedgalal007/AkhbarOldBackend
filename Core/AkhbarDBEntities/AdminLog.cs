// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.AdminLog
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  [Table("AdminLog")]
  public class AdminLog
  {
    [Key]
    public int LogID { get; set; }

    public int? LogUserID { get; set; }

    public DateTime? LogDate { get; set; }

    [StringLength(200)]
    [Display(Name = "مسار ملف اللوج")]
    public string LogURL { get; set; }

    [StringLength(50)]
    public string LogRemoteAddress { get; set; }

    [StringLength(50)]
    public string LogServerName { get; set; }

    [StringLength(200)]
    public string LogQueryString { get; set; }

    public virtual AdminUser AdminUser { get; set; }
  }
}

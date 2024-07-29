// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.TraceTable
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  [Table("TraceTable")]
  public class TraceTable
  {
    [Key]
    public int RowNumber { get; set; }

    public int? EventClass { get; set; }

    [StringLength(128)]
    public string ApplicationName { get; set; }

    [StringLength(128)]
    public string NTUserName { get; set; }

    [StringLength(128)]
    public string LoginName { get; set; }

    public int? CPU { get; set; }

    public long? Reads { get; set; }

    public long? Writes { get; set; }

    public long? Duration { get; set; }

    public int? ClientProcessID { get; set; }

    public int? SPID { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    [Column(TypeName = "image")]
    public byte[] BinaryData { get; set; }
  }
}

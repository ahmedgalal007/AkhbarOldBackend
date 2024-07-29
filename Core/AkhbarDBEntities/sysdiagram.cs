// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.sysdiagram
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class sysdiagram
  {
    [Required]
    [StringLength(128)]
    public string name { get; set; }

    public int principal_id { get; set; }

    [Key]
    public int diagram_id { get; set; }

    public int? version { get; set; }

    public byte[] definition { get; set; }
  }
}

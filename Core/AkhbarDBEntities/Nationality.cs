// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Nationality
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  [Table("Nationality")]
  public class Nationality
  {
    [Key]
    public short CountryID { get; set; }

    [Required]
    [StringLength(20)]
    public string Name { get; set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.RelatedNew
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  public class RelatedNew
  {
    [Key]
    [Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int NewID { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int RelatedId { get; set; }
  }
}

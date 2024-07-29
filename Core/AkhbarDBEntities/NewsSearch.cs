// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.NewsSearch
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  [Table("NewsSearch")]
  public class NewsSearch
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int NewID { get; set; }

    [Required]
    [StringLength(200)]
    public string title { get; set; }

    [StringLength(150)]
    public string byline { get; set; }

    [StringLength(3000)]
    public string brief { get; set; }

    public int SectionID { get; set; }

    [StringLength(200)]
    public string title2 { get; set; }

    [StringLength(3000)]
    public string brief2 { get; set; }

    [StringLength(150)]
    public string byline2 { get; set; }
  }
}

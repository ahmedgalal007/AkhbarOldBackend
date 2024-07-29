// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.vTopNewsToday
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  [Table("vTopNewsToday")]
  public class vTopNewsToday
  {
    public int? Views { get; set; }

    [StringLength(8000)]
    public string PublishDate { get; set; }

    public string Brief { get; set; }

    [StringLength(200)]
    public string Title { get; set; }

    [Key]
    [Column(Order = 0)]
    [StringLength(100)]
    public string PictureName { get; set; }

    [Key]
    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int NewID { get; set; }

    [StringLength(200)]
    public string SubTitle { get; set; }
  }
}

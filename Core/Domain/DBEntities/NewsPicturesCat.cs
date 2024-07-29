// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.NewsPicturesCat
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  public class NewsPicturesCat
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CatID { get; set; }

    [StringLength(200)]
    public string CatName { get; set; }
  }
}

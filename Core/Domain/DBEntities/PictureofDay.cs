// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.PictureofDay
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  [Table("PictureofDay")]
  public class PictureofDay
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int PictureID { get; set; }

    [Required]
    [StringLength(200)]
    public string PictureTitle { get; set; }

    public DateTime Date_Added { get; set; }

    public int JournalID { get; set; }
  }
}

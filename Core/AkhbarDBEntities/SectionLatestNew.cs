// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.SectionLatestNew
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class SectionLatestNew
  {
    [Key]
    public int NewID { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; }

    [StringLength(3000)]
    public string Brief { get; set; }

    [Required]
    [StringLength(50)]
    public string PictureName { get; set; }

    [StringLength(1000)]
    public string PictureCaption1 { get; set; }

    public DateTime? PublishDate { get; set; }

    [StringLength(50)]
    public string SecTitle { get; set; }

    public byte? WeeklySection { get; set; }
  }
}

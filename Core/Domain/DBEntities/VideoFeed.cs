// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.VideoFeed
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Akhbar.DBEntities
{
  public class VideoFeed
  {
    [Key]
    public int EntryID { get; set; }

    [StringLength(150)]
    public string Title { get; set; }

    [StringLength(1000)]
    public string Brief { get; set; }

    [StringLength(100)]
    public string Link { get; set; }

    [StringLength(100)]
    public string Type { get; set; }

    public DateTime? Date { get; set; }

    [StringLength(100)]
    public string Thumb { get; set; }

    [StringLength(150)]
    public string TitleAR { get; set; }

    [StringLength(1000)]
    public string BriefAR { get; set; }

    [StringLength(100)]
    public string TypeAR { get; set; }
  }
}

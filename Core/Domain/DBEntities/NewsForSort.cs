// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.NewsForSort
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;

namespace Domain.Akhbar.DBEntities
{
  public class NewsForSort
  {
    public int NewID { get; set; }

    public int? EditorID { get; set; }

    public string EditorName { get; set; }

    public string Title { get; set; }

    public int? DisplayOrder { get; set; }

    public int? PictureID1 { get; set; }

    public string SecTitle { get; set; }

    public DateTime? PublishDate { get; set; }
  }
}

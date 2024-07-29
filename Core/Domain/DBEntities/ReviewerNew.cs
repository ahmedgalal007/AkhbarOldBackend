// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.ReviewerNew
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;

namespace Domain.Akhbar.DBEntities
{
  public class ReviewerNew
  {
    public long ID { get; set; }

    public int? NewId { get; set; }

    public int? UserId { get; set; }

    public int? Done { get; set; }

    public DateTime? getNewDate { get; set; }
  }
}

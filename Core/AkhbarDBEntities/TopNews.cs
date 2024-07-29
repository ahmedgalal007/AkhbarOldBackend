// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.TopNews
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  public class TopNews
  {
    public int NewsID { get; set; }

    public int SecID { get; set; }

    public int CatID { get; set; }

    public int DisplayOrder { get; set; }

    public int? JournalID { get; set; }

    public long ID { get; set; }

    [ForeignKey("NewsID")]
    public virtual News News { get; set; }

    [ForeignKey("SecID")]
    public virtual MainSection MainSection { get; set; }
  }
}

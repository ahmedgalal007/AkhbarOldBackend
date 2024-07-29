// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.m_SP_SelectNewsToArrange
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

namespace Akhbar.DBEntities
{
  public class m_SP_SelectNewsToArrange : IStoredProcedure<NewsForSort>
  {
    public int CatID { get; set; }

    public int SectionID { get; set; }

    public int JournalID { get; set; }
  }
}

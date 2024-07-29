// Decompiled with JetBrains decompiler
// Type: Akhbar.DBBusiness.NewsGalleryBusiness
// Assembly: AkhbarDBBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 447873D6-3586-48DC-A4C8-11855DFF0A7A
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBBusiness.dll

using Akhbar.DBEntities;
using System.Data.Entity;

namespace Akhbar.DBBusiness
{
  public class NewsGalleryBusiness : BaseBusiness.BaseBusiness<NewsGallery>
  {
    public NewsGalleryBusiness()
    {
    }

    public NewsGalleryBusiness(DbContext _DbContext)
      : base(_DbContext)
    {
    }
  }
}

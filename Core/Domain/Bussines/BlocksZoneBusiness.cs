// Decompiled with JetBrains decompiler
// Type: Akhbar.DBBusiness.BlocksZoneBusiness
// Assembly: AkhbarDBBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 447873D6-3586-48DC-A4C8-11855DFF0A7A
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBBusiness.dll

using Domain.Akhbar.DBEntities;
using System.Data.Entity;

namespace Domain.Akhbar.DBBusiness
{
  public class BlocksZoneBusiness : BaseBusiness.BaseBusiness<BlocksZone>
  {
    public BlocksZoneBusiness()
    {
    }

    public BlocksZoneBusiness(DbContext _DbContext)
      : base(_DbContext)
    {
    }
  }
}

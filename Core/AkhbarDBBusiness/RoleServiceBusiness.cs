﻿// Decompiled with JetBrains decompiler
// Type: Akhbar.DBBusiness.RoleServiceBusiness
// Assembly: AkhbarDBBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 447873D6-3586-48DC-A4C8-11855DFF0A7A
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBBusiness.dll

using Akhbar.DBEntities;
using System.Data.Entity;

namespace Akhbar.DBBusiness
{
  public class RoleServiceBusiness : BaseBusiness.BaseBusiness<RoleServices>
  {
    public RoleServiceBusiness()
    {
    }

    public RoleServiceBusiness(DbContext _DbContext)
      : base(_DbContext)
    {
    }
  }
}
// Decompiled with JetBrains decompiler
// Type: Akhbar.DBBusiness.AdminUserBusiness
// Assembly: AkhbarDBBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 447873D6-3586-48DC-A4C8-11855DFF0A7A
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBBusiness.dll

using Domain.Akhbar.DBEntities;
using LinqKit;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Akhbar.DBBusiness
{
  public class AdminUserBusiness : BaseBusiness.BaseBusiness<AdminUser>
  {
    public AdminUserBusiness()
    {
    }

    public AdminUserBusiness(DbContext _DbContext)
      : base(_DbContext)
    {
    }

    public IPagedList<AdminUser> Load(
      Expression<Func<AdminUser, bool>> user,
      int pageNumber,
      int pageSize,
      params Expression<Func<AdminUser, object>>[] includeExpressions)
    {
      if (user == null)
        user = (Expression<Func<AdminUser, bool>>) (c => true);
      IQueryable<AdminUser> source = this.DbContext.Set<AdminUser>().AsExpandable<AdminUser>().SelectMany((Expression<Func<AdminUser, IEnumerable<UserRole>>>) (a => a.UserRoleLst), (a, b) => new
      {
        a = a,
        b = b
      }).Where(data => user.Invoke<AdminUser, bool>(data.a)).Select(data => data.a);
      if (includeExpressions != null)
      {
        foreach (Expression<Func<AdminUser, object>> includeExpression in includeExpressions)
          source = source.Include<AdminUser, object>(includeExpression);
      }
      return source.OrderBy<AdminUser, int>((Expression<Func<AdminUser, int>>) (f => f.UserID)).ToPagedList<AdminUser>(pageNumber, pageSize);
    }

    public List<VUserPermissions> LoadUserPermissions(AdminUser _User) => this.DbContext.Set<VUserPermissions>().Where<VUserPermissions>((Expression<Func<VUserPermissions, bool>>) (p => p.UserID == _User.UserID)).ToList<VUserPermissions>();
  }
}

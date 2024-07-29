// Decompiled with JetBrains decompiler
// Type: BaseBusiness.BaseBusiness`1
// Assembly: BaseBusiness, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D50DAB97-85B9-4020-A77A-3C82CE0783B6
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\BaseBusiness.dll

using EntityFramework.DynamicFilters;
using LinqKit;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BaseBusiness
{
  public class BaseBusiness<T> where T : class
  {
    public BaseBusiness()
    {
    }

    public BaseBusiness(DbContext _DbContext) => this.DbContext = _DbContext;

    public virtual DbContext DbContext { get; set; }

    public virtual void EnableDeleteFilter(bool enable, bool isDeleted = false)
    {
      if (this.DbContext == null)
        return;
      if (enable)
      {
        this.DbContext.EnableFilter("IsDeletedFilter");
        this.DbContext.SetFilterScopedParameterValue("IsDeletedFilter", (object) isDeleted);
      }
      else
        this.DbContext.DisableFilter("IsDeletedFilter");
    }

    public virtual void EnableUserFilter(bool enable, List<int> UsersIDs = null)
    {
      if (this.DbContext == null)
        return;
      if (enable)
      {
        this.DbContext.EnableFilter("UserIDsFilter");
        if (UsersIDs == null)
        {
          UsersIDs = new List<int>();
          if (HttpContext.Current != null && HttpContext.Current.Session["UserID"] != null)
          {
            List<int> collection = new List<int>()
            {
              (int) HttpContext.Current.Session["UserID"]
            };
            UsersIDs.AddRange((IEnumerable<int>) collection);
          }
        }
        this.DbContext.SetFilterScopedParameterValue("UserIDsFilter", "valueList", (object) UsersIDs);
      }
      else
        this.DbContext.DisableFilter("UserIDsFilter");
    }

    public virtual T Load(int? ID) => this.DbContext.Set<T>().Find(new object[1]
    {
      (object) ID
    });

    public virtual T Load(string ID) => this.DbContext.Set<T>().Find(new object[1]
    {
      (object) ID
    });

    public virtual List<T> Load(
      Expression<Func<T, bool>> filter,
      params Expression<Func<T, object>>[] includeExpressions)
    {
      if (filter == null)
        filter = (Expression<Func<T, bool>>) (c => true);
      IQueryable<T> source = this.DbContext.Set<T>().AsExpandable<T>().Where<T>((Expression<Func<T, bool>>) (a => filter.Invoke<T, bool>(a)));
      if (includeExpressions != null)
      {
        foreach (Expression<Func<T, object>> includeExpression in includeExpressions)
          source = source.Include<T, object>(includeExpression);
      }
      return source.ToList<T>();
    }

    public virtual List<T> LoadDesc(
      Expression<Func<T, int>> keySelector,
      int limit,
      Expression<Func<T, bool>> filter,
      params Expression<Func<T, object>>[] includeExpressions)
    {
      if (filter == null)
        filter = (Expression<Func<T, bool>>) (c => true);
      IQueryable<T> source = this.DbContext.Set<T>().AsExpandable<T>().OrderByDescending<T, int>(keySelector).Where<T>((Expression<Func<T, bool>>) (a => filter.Invoke<T, bool>(a)));
      if (includeExpressions != null)
      {
        foreach (Expression<Func<T, object>> includeExpression in includeExpressions)
          source = source.Include<T, object>(includeExpression);
      }
      return source.Take<T>(limit).ToList<T>();
    }

    public virtual IPagedList<T> LoadDesc(
      Expression<Func<T, int>> keySelector,
      int limit,
      Expression<Func<T, bool>> filter,
      int pageNumber,
      int pageSize,
      params Expression<Func<T, object>>[] includeExpressions)
    {
      if (filter == null)
        filter = (Expression<Func<T, bool>>) (c => true);
      IQueryable<T> source = this.DbContext.Set<T>().AsExpandable<T>().OrderByDescending<T, int>(keySelector).Where<T>((Expression<Func<T, bool>>) (a => filter.Invoke<T, bool>(a)));
      if (includeExpressions != null)
      {
        foreach (Expression<Func<T, object>> includeExpression in includeExpressions)
          source = source.Include<T, object>(includeExpression);
      }
      return source.Take<T>(limit).ToPagedList<T>(pageNumber, pageSize);
    }

    public virtual IPagedList<T> LoadDesc(
      Expression<Func<T, DateTime>> keySelector,
      int limit,
      Expression<Func<T, bool>> filter,
      int pageNumber,
      int pageSize,
      params Expression<Func<T, object>>[] includeExpressions)
    {
      if (filter == null)
        filter = (Expression<Func<T, bool>>) (c => true);
      IQueryable<T> source = this.DbContext.Set<T>().AsExpandable<T>().OrderByDescending<T, DateTime>(keySelector).Where<T>((Expression<Func<T, bool>>) (a => filter.Invoke<T, bool>(a)));
      if (includeExpressions != null)
      {
        foreach (Expression<Func<T, object>> includeExpression in includeExpressions)
          source = source.Include<T, object>(includeExpression);
      }
      return source.Take<T>(limit).ToPagedList<T>(pageNumber, pageSize);
    }

    public virtual IPagedList<T> Load(
      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
      Expression<Func<T, bool>> filter,
      int pageNumber,
      int pageSize,
      params Expression<Func<T, object>>[] includeExpressions)
    {
      if (filter == null)
        filter = (Expression<Func<T, bool>>) (c => true);
      IQueryable<T> source = this.DbContext.Set<T>().Distinct<T>().AsExpandable<T>().Where<T>((Expression<Func<T, bool>>) (a => filter.Invoke<T, bool>(a)));
      if (includeExpressions != null)
      {
        foreach (Expression<Func<T, object>> includeExpression in includeExpressions)
          source = source.Include<T, object>(includeExpression);
      }
      return orderBy(source).ToPagedList<T>(pageNumber, pageSize);
    }

    public virtual void Add(T _entity) => this.DbContext.Set<T>().Add(_entity);

    public virtual T AddIfNotExists(T _entity, Func<T, bool> predicate = null) => !(predicate != null ? this.DbContext.Set<T>().Any<T>(predicate) : this.DbContext.Set<T>().Any<T>()) ? this.DbContext.Set<T>().Add(_entity) : default (T);

    public virtual void Edit(T _entity) => this.DbContext.Entry<T>(_entity).State = EntityState.Modified;

    public virtual void EditSingleCol(
      T _entity,
      params Expression<Func<T, object>>[] updatedProperties)
    {
      this.DbContext.Entry<T>(_entity).State = EntityState.Unchanged;
      foreach (Expression<Func<T, object>> updatedProperty in updatedProperties)
        this.DbContext.Entry<T>(_entity).Property<object>(updatedProperty).IsModified = true;
    }

    public virtual void Edit(T _entity, string[] prob)
    {
      this.DbContext.Entry<T>(_entity).State = EntityState.Unchanged;
      foreach (string propertyName in prob)
        this.DbContext.Entry<T>(_entity).Property(propertyName).IsModified = true;
    }

    public virtual void Delete(T _entity) => this.DbContext.Entry<T>(_entity).State = EntityState.Deleted;

    public virtual bool Exists(T _entity) => this.DbContext.Set<T>().Local.Any<T>((Func<T, bool>) (e => (object) e == (object) _entity));

    public virtual bool Exists(params object[] keys) => (object) this.DbContext.Set<T>().Find(keys) != null;
  }
}

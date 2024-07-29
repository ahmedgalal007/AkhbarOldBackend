// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.DatabaseExtensions
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Akhbar.DBEntities
{
  public static class DatabaseExtensions
  {
    private const bool RetrieveNameFromResultDto = false;

    public static IEnumerable<TResult> ExecuteStoredProcedure<TResult>(
      this Database database,
      IStoredProcedure<TResult> procedure)
    {
      List<SqlParameter> parametersFromProperties = DatabaseExtensions.CreateSqlParametersFromProperties<TResult>(procedure);
      string spCommand = DatabaseExtensions.CreateSPCommand<TResult>(procedure.GetType().Name, parametersFromProperties);
      return (IEnumerable<TResult>) database.SqlQuery<TResult>(spCommand, parametersFromProperties.Cast<object>().ToArray<object>());
    }

    private static List<SqlParameter> CreateSqlParametersFromProperties<TResult>(
      IStoredProcedure<TResult> procedure)
    {
      return ((IEnumerable<PropertyInfo>) procedure.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)).Select<PropertyInfo, SqlParameter>((Func<PropertyInfo, SqlParameter>) (propertyInfo => DatabaseExtensions.ConvertPropertyToSpParameter<TResult>(procedure, propertyInfo))).ToList<SqlParameter>();
    }

    private static SqlParameter ConvertPropertyToSpParameter<TResult>(
      IStoredProcedure<TResult> procedure,
      PropertyInfo propertyInfo)
    {
      return new SqlParameter(string.Format("@{0}", (object) propertyInfo.Name), propertyInfo.GetValue((object) procedure, new object[0]));
    }

    private static string CreateSPCommand<TResult>(
      string storedProcedureName,
      List<SqlParameter> parameters)
    {
      string queryString = string.Format("{0}", (object) storedProcedureName);
      parameters.ForEach((Action<SqlParameter>) (x => queryString = string.Format("{0} {1},", (object) queryString, (object) x.ParameterName)));
      return queryString.TrimEnd(',');
    }

    private static string CreateStoredProcedureName<TResult>()
    {
      string str = typeof (TResult).Name;
      if (str.EndsWith("Result"))
        str = str.Substring(0, str.LastIndexOf("Result"));
      return string.Format("{0}", (object) str);
    }
  }
}
